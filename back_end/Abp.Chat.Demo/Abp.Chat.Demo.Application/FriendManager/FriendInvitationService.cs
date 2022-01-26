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
    [ApiExplorerSettings(GroupName = "好友邀请管理")]
    public class FriendInvitationService : ApplicationService, IFriendInvitationService
    {
        private readonly IRedisDatabase _redis;
        private readonly ICustomUser _user;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly AbpChatRemoteCall _abpChatRemoteCall;
        private readonly IGuidGenerator _guidGenerator;
        public FriendInvitationService(IRedisDatabase redis,
            ICustomUser user,
            IInvitationRepository invitationRepository,
            IUserRepository userRepository,
            IFriendRepository friendRepository,
            AbpChatRemoteCall abpChatRemoteCall,
             IGuidGenerator guidGenerator
            )
        {
            _redis = redis;
            _user = user;
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _friendRepository = friendRepository;
            _abpChatRemoteCall = abpChatRemoteCall;
            _guidGenerator = guidGenerator;
        }
        [CustomApi(RelationPath = "/FriendInvitation/AddInvitaion", Description = "添加邀请")]
        public async Task<ApiResult<object>> AddInvitaion(InvitationDto invitationDto)
        {
            //先去查找数据库中是否有这个用户，如果没有则返回错误
            var IsExistUser = _userRepository.Where(u => u.Id.Equals(invitationDto.FriendId)).Any();
            if (!IsExistUser)
            {
                throw new AbpApiException("不存在该用户");
            }
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            //如果对方已经是自己的好友，则无法发起邀请
            var IsExistFriend = await _friendRepository.Where(f =>
            f.UserId.Equals(invitationDto.FriendId) &&
            f.ChumId.Equals(user.Id) &&
            !f.IsDeleted).AnyAsync();
            if (IsExistFriend)
            {
                throw new AbpApiException("对方已经是好友");
            }
            var invitation = new Invitation
            {
                UserId = user.Id,
                ChumId = invitationDto.FriendId
            };
            invitation.SetInvitationId(_guidGenerator.Create());
            //如果已经存在相同邀请或者且没有被拒绝则不进行添加邀请
            var isExistInvitation = _invitationRepository.
                Where(i => i.ChumId.Equals(invitation.ChumId) &&
                i.UserId.Equals(invitation.UserId) &&
                i.IsAccept == InvitationAcceptState.NONE)
                .Any();
            if (isExistInvitation)
            {
                throw new AbpApiException("邀请已经存在");
            }
            try
            {
                await _invitationRepository.InsertAsync(invitation);
                await _abpChatRemoteCall.AddInvitaionAsync(invitationDto.FriendId);
            }
            catch
            {
                throw new AbpApiException("添加邀请失败");
            }
            return await ApiResult<object>.GetResultAsync(null);
        }
        [CustomApi(RelationPath = "/FriendInvitation/DelInvitation", Description = "删除单个邀请")]
        public async Task<ApiResult<object>> DelInvitation(RefusedInvitationDto invitationDto)
        {
            var invitation = await _invitationRepository.Where(i => i.Id.Equals(invitationDto.InvitationId)).FirstOrDefaultAsync();
            if (invitation == null)
            {
                throw new AbpApiException("没找到邀请");
            }
            await _invitationRepository.DeleteAsync(invitation);
            return await ApiResult<object>.GetResultAsync(null);
        }

        [CustomApi(RelationPath = "/FriendInvitation/FindAllInvitation", Description = "查找当前用户所有好友邀请")]
        public async Task<ApiResult<List<FriendInvitationDto>>> FindAllInvitation()
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var FriendInvitationDtoList = from i in _invitationRepository.AsQueryable()
                                          where i.ChumId.Equals(user.Id)
                                          join u in _userRepository.AsQueryable()
                                          on i.UserId equals u.Id
                                          select new FriendInvitationDto
                                          {
                                              Id = i.Id,
                                              UserId = i.UserId,
                                              UserName = u.UserName,
                                              ChumId = i.ChumId,
                                              CreatedTime = i.CreatedTime,
                                              IsAccept = i.IsAccept,
                                              IsReaded = i.IsReaded
                                          };
            var list = await FriendInvitationDtoList.OrderByDescending(f => f.CreatedTime).ToListAsync();
            return await ApiResult<List<FriendInvitationDto>>.GetResultAsync(list);
        }
        [CustomApi(RelationPath = "/FriendInvitation/FindNewInvitation", Description = "查询未读邀请信息数量")]
        public async Task<ApiResult<int>> FindNewInvitation()
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var count = await _invitationRepository.Where(i => !i.IsReaded && i.ChumId.Equals(user.Id)).CountAsync();
            return await ApiResult<int>.GetResultAsync(count);
        }

        [CustomApi(RelationPath = "/FriendInvitation/ReadAllInvitation", Description = "已读所有邀请")]
        public async Task<ApiResult<object>> ReadAllInvitation()
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var resSearch = _invitationRepository.Where(i => i.ChumId.Equals(user.Id) && !i.IsReaded);
            if (!resSearch.Any())
            {
                return await ApiResult<object>.GetResultAsync(null);
            }
            await _invitationRepository.SetInvitationReadedAsync(user.Id);
            return await ApiResult<object>.GetResultAsync(null);
        }
        [CustomApi(RelationPath = "/FriendInvitation/RefusedInvitation", Description = "拒绝好友邀请")]
        public async Task<ApiResult<object>> RefusedInvitation(RefusedInvitationDto refusedInvitationDto)
        {
            var invitation = await _invitationRepository.Where(i => i.Id.Equals(refusedInvitationDto.InvitationId)).FirstOrDefaultAsync();
            if (invitation == null)
            {
                throw new AbpApiException("没找到邀请");
            }
            invitation.IsAccept = InvitationAcceptState.REFUASED;
            return await ApiResult<object>.GetResultAsync(null);
        }
    }
}
