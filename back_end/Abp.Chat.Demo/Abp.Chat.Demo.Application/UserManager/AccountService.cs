using Abp.Chat.Demo.Contract.Dto;
using Abp.Chat.Demo.Contract.Interfaces;
using Abp.Chat.Demo.Contract.IRepository;
using Abp.Chat.Demo.Domain;
using Custom.Extensions;
using Custom.Extensions.AuthHelper;
using Custom.Extensions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;

namespace Abp.Chat.Demo.Application.UserManager
{
    [ApiExplorerSettings(GroupName = "用户管理")]
    public class AccountService : ApplicationService, IAccountService
    {
        private readonly IRedisDatabase _redis;
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<AccountService> _logger;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICustomUser _user;
        public AccountService(IRedisDatabase redis,
            IUserRepository userRepository,
            TokenHelper tokenHelper,
            ILogger<AccountService> logger,
            IGuidGenerator guidGenerator,
            ICustomUser user)
        {
            _redis = redis;
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _logger = logger;
            _guidGenerator = guidGenerator;
            _user = user;
        }
        [Authorize("Need_Auth_API")]
        [CustomApi(RelationPath = "/Account/ChangePass", Description = "修改密码")]
        public async Task<ApiResult<object>> ChangePass(ChangePasswordDto changePasswordDto)
        {
            var currentUser = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            //新密码
            var newPass = Md5Helper.Md5(changePasswordDto.NewPassword);
            //旧密码
            var oldPass = Md5Helper.Md5(changePasswordDto.OldPassword);
            //查找用户
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id.Equals(currentUser.Id));
            if (!oldPass.Equals(user.Password))
            {
                throw new AbpApiException("旧密码错误");
            }
            user.Password = newPass;
            //改完密码得重新登录
            await DeleteRedisCache(_user.SessionId);
            return await ApiResult<object>.GetResultAsync(null);
        }

        [Authorize("Need_Auth_API")]
        [CustomApi(RelationPath = "/Account/FindUsers", Description = "查找用户", Method = "GET")]
        public async Task<ApiResult<List<RedisUserDto>>> FindUsers([Required] string searchStr)
        {
            //IMPROVE 后面考虑建个父类然后把当前用户数据放到里面去
            var currentUser = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var users = await _userRepository
                .Where(u => u.UserName.Contains(searchStr) && !u.UserName.Equals(currentUser.UserName))
                .ToListAsync();
            var userList = ObjectMapper.Map<List<User>, List<RedisUserDto>>(users);
            return await ApiResult<List<RedisUserDto>>.GetResultAsync(userList);
        }

        [CustomApi(RelationPath = "/Account/Login", Description = "登录")]
        public async Task<ApiResult<object>> Login(UserDto user)
        {
            var _user = await _userRepository.FirstOrDefaultAsync(u => user.UserName.Equals(u.UserName)
             && Md5Helper.Md5(user.Password).Equals(u.Password));
            if (_user == null)
            {
                throw new AbpApiException("登陆失败，密码或用户名错误。");
            }
            //去redis中查找相关的缓存如果存在则清除
            var userSessionId = _user.SessionId ?? "";
            await DeleteRedisCache(userSessionId);
            //生成uuid
            var sessionId = Guid.NewGuid().ToString("N");
            //生成新的redis缓存
            return await GetTokenResult(sessionId, _user);
        }

        private async Task DeleteRedisCache(string userSessionId)
        {
            var redisUser = await _redis.GetAsync<RedisUserDto>(userSessionId);
            if (redisUser != null)
            {
                await _redis.RemoveAsync(userSessionId);
            }
        }

        [Authorize("Need_Auth_API")]
        [CustomApi(RelationPath = "/Account/Loginout", Description = "注销", Method = "Get")]
        public async Task<ApiResult<object>> Loginout()
        {
            //直接去除redis中的缓存
            var isSuccess = await _redis.RemoveAsync(_user.SessionId);
            if (isSuccess)
            {
                return await ApiResult<object>.GetResultAsync(null);
            }
            throw new AbpApiException("退出失败");
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [CustomApi(RelationPath = "/Account/Register", Description = "注册")]
        public async Task<ApiResult<object>> Register(UserDto user)
        {
            //存在相同用户名
            var IsExist = _userRepository.Where(u => u.UserName.Equals(user.UserName)).Any();
            if (IsExist)
            {
                //UNDONE 后面可能添加类似微信号这种东西当账号 
                throw new AbpApiException("用户名已存在");
            }
            //生成uuid
            var sessionId = Guid.NewGuid().ToString("N");
            var newUser = ObjectMapper.Map<UserDto, User>(user);
            var guid = _guidGenerator.Create();
            newUser.SetUserId(guid);
            newUser.Password = Md5Helper.Md5(newUser.Password);
            //修改sessionId为当前创建的sessionId
            newUser.SessionId = sessionId;
            var entity = await _userRepository.InsertAsync(newUser);
            return await GetTokenResult(sessionId, entity);
        }

        private async Task<ApiResult<object>> GetTokenResult(string sessionId, User entity)
        {
            entity.SessionId = sessionId;
            //30分钟过期,跟token时间对应
            await _redis.AddAsync(sessionId, ObjectMapper.Map<User, RedisUserDto>(entity), TimeSpan.FromMinutes(30));
            //把sessionId放入token返回给前端
            var token = _tokenHelper.TokenCreate(new Dictionary<string, string>
            {
                { "SessionId",sessionId}
            });
            _logger.LogInformation($"用户sessionId:{sessionId}");
            return await ApiResult<object>.GetResultAsync(new
            {
                token = token,
                userId = entity.Id,
                userName = entity.UserName,
                header = entity.HeaderImg
            });
        }

    }
}
