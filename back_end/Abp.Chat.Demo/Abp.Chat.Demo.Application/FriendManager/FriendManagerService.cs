using Abp.Chat.Demo.Application.Hubs;
using Abp.Chat.Demo.Contract.Dto;
using Abp.Chat.Demo.Contract.Interfaces;
using Abp.Chat.Demo.Contract.IRepository;
using Abp.Chat.Demo.Domain;
using Custom.Extensions;
using Custom.Extensions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;

namespace Abp.Chat.Demo.Application.FriendManager
{
    [Authorize("Need_Auth_API")]
    [ApiExplorerSettings(GroupName = "好友管理")]
    public class FriendManagerService : ApplicationService, IFriendManagerService
    {
        private readonly IRedisDatabase _redis;
        private readonly ICustomUser _user;
        private readonly IFriendRepository _friendRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly AbpChatRemoteCall _abpChatRemoteCall;
        private readonly IGuidGenerator _guidGenerator;
        public FriendManagerService(IRedisDatabase redis,
            ICustomUser user,
            IFriendRepository friendRepository,
            IUserRepository userRepository,
            IInvitationRepository invitationRepository,
            AbpChatRemoteCall abpChatRemoteCall,
            IGuidGenerator guidGenerator)
        {
            _redis = redis;
            _user = user;
            _friendRepository = friendRepository;
            _userRepository = userRepository;
            _invitationRepository = invitationRepository;
            _abpChatRemoteCall = abpChatRemoteCall;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        ///  当用户点击同意好友邀请时才能使用
        /// </summary>
        /// <param name="invitationDto">包含发来的邀请的用户Id和邀请Id</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [CustomApi(RelationPath = "/FriendManager/AddFriend", Description = "添加好友")]
        public async Task<ApiResult<AddFriendDto>> AddFriend(InvitationFriendDto invitationDto)
        {
            //分两种结果返回，一种websocket,一种接口直接返回
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            //查看是否存在用户,并且不是当前用户
            if (invitationDto.FriendId.Equals(user.Id))
            {
                throw new AbpApiException("不能添加自己为自己的好友");
            }
            var userQuery = _userRepository.Where(u => u.Id.Equals(invitationDto.FriendId));
            var invitationQuery = _invitationRepository.Where(i => i.Id.Equals(invitationDto.InvitationId));
            //查找邀请
            var invitationIsExist = await invitationQuery.AnyAsync();
            var userIsExist = await userQuery.AnyAsync();
            if (!userIsExist || !invitationIsExist)
            {
                throw new AbpApiException("用户不存在或者好友邀请不存在");
            }
            var IsExistFriend = await _friendRepository.Where(f =>
             f.UserId.Equals(user.Id) &&
             f.ChumId.Equals(invitationDto.FriendId) &&
             !f.IsDeleted).AnyAsync();
            if (IsExistFriend)
            {
                throw new AbpApiException("好友已经存在");
            }
            var friend = await userQuery.FirstAsync();
            //正常来说需要添加两条记录
            var newFriend = new Friend
            {
                UserId = user.Id,
                ChumId = invitationDto.FriendId,
                Nickname = friend.UserName,
                IsDeleted = false
            };
            newFriend.SetFriendId(_guidGenerator.Create());
            var newChum = new Friend
            {
                UserId = invitationDto.FriendId,
                ChumId = user.Id,
                Nickname = user.UserName,
                IsDeleted = false
            };
            newChum.SetFriendId(_guidGenerator.Create());
            //添加数据
            await _friendRepository.InsertManyAsync(new List<Friend> { newFriend, newChum });
            await _invitationRepository.AggreeInvitationAsync(invitationDto.InvitationId);
            // IMPROVE 到时候添加一个排序字段，通过这个字段判断把哪个好友置顶,最新消息先不管
            await _abpChatRemoteCall.AddNewFriendAsync(invitationDto.FriendId, user);
            //接口返回,当前用户接收
            return await ApiResult<AddFriendDto>.GetResultAsync(new AddFriendDto { UserId = invitationDto.FriendId, Nickname = friend.UserName });
        }
        /// <summary>
        /// TODO 接口未测试
        /// </summary>
        /// <param name="friendDto"></param>
        /// <returns></returns>
        [CustomApi(RelationPath = "/FriendManager/DelFriend", Description = "删除好友")]
        public async Task<ApiResult<object>> DelFriend(DelFriendDto friendDto)
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var delFriend = await _friendRepository.Where(f => f.UserId.Equals(user.Id) && f.ChumId.Equals(friendDto.FriendId)).FirstOrDefaultAsync();
            delFriend.IsDeleted = true;
            return await ApiResult<object>.GetResultAsync(null);
        }
        /// <summary>
        /// 查找好友
        /// </summary>
        /// <returns></returns>
        [CustomApi(RelationPath = "/FriendManager/FindFriends", Description = "查找好友")]
        public async Task<ApiResult<List<AddFriendDto>>> FindFriends()
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var result = await _friendRepository.Where(f => f.UserId.Equals(user.Id) && !f.IsDeleted).Select(f => new AddFriendDto
            {
                UserId = f.ChumId,
                Nickname = f.Nickname
            }).ToListAsync();
            return await ApiResult<List<AddFriendDto>>.GetResultAsync(result);
        }
        /// <summary>
        /// TODO 接口未测试
        /// </summary>
        /// <param name="friendDto"></param>
        /// <returns></returns>
        [CustomApi(RelationPath = "/FriendManager/UpdateFriendsNickname", Description = "修改好友昵称")]
        public async Task<ApiResult<object>> UpdateFriendsNickname(UpdateFriendDto friendDto)
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var updateFriend = await _friendRepository.Where(f => f.UserId.Equals(user.Id) && f.ChumId.Equals(friendDto.FriendId)).FirstOrDefaultAsync();
            updateFriend.Nickname = friendDto.NickName;
            return await ApiResult<object>.GetResultAsync(null);
        }
    }
}
