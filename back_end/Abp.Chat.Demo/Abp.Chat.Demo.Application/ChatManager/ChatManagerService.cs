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
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Abp.Chat.Demo.Application.ChatManager
{
    [Authorize("Need_Auth_API")]
    [ApiExplorerSettings(GroupName = "聊天管理")]
    public class ChatManagerService : ApplicationService, IChatManagerService
    {
        private readonly IRedisDatabase _redis;
        private readonly ICustomUser _user;
        private readonly IChatInformationRepository _chatInformation;
        private readonly IUserRepository _userRepository;
        private readonly AbpChatRemoteCall _abpChatRemoteCall;
        private readonly IGuidGenerator _guidGenerator;
        public ChatManagerService(IRedisDatabase redis,
            ICustomUser user,
            IChatInformationRepository chatInformation,
            IUserRepository userRepository,
            AbpChatRemoteCall abpChatRemoteCall,
            IGuidGenerator guidGenerator)
        {
            _redis = redis;
            _user = user;
            _chatInformation = chatInformation;
            _userRepository = userRepository;
            _abpChatRemoteCall = abpChatRemoteCall;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 查找所有未读消息并分组
        /// </summary>
        /// <returns></returns>
        [CustomApi(RelationPath = "/ChatManager/FindAllNewMessage", Description = "查找所有未读消息并分组")]
        public async Task<ApiResult<List<GroupInformationCountDto>>> FindAllNewMessage()
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            //只要接收人是当前用户则查出来
            var informationGroup = await _chatInformation
                .Where(c => c.RecivedId.Equals(user.Id) && !c.IsRead)
                .GroupBy(c => c.SenderId).Select(c => new GroupInformationCountDto
                {
                    UserId = c.Key,
                    UnRead = c.Count()
                })
                .ToListAsync();
            return await ApiResult<List<GroupInformationCountDto>>.GetResultAsync(informationGroup);
        }
        /// <summary>
        /// 根据用户Id查找聊天信息并设置已读
        /// </summary>
        /// <param name="chatMsgReciveDto"></param>
        /// <returns></returns>
        [CustomApi(RelationPath = "/ChatManager/FindMsgsAndReadByFriend", Description = "根据用户Id查找聊天信息并设置已读")]
        public async Task<ApiResult<List<ChatMsgDto>>> FindMsgsAndReadByFriend(ChatMsgReciveDto chatMsgReciveDto)
        {
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            //只查找未读信息
            var query = _chatInformation
                        .Where(c =>
                              c.SenderId.Equals(chatMsgReciveDto.CurrentChatUserId) &&
                              c.RecivedId.Equals(user.Id) &&
                              !c.IsRead)
                        .OrderBy(c => c.CreatedTime);
            var chatMsgList = await query.ToListAsync();
            //将消息设置成已读
            foreach (var chat in chatMsgList)
            {
                chat.IsRead = true;
            }
            var chatList = chatMsgList.Select(c => new ChatMsgDto
            {
                Id = c.Id,
                UserId = c.SenderId,
                IsMe = c.SenderId.Equals(user.Id),
                Message = c.Message,
                SendTime = c.CreatedTime
            }).ToList();
            return await ApiResult<List<ChatMsgDto>>.GetResultAsync(chatList);
        }
        [CustomApi(RelationPath = "/ChatManager/ReadSingleMessage", Description = "已读单条信息")]
        public async Task<ApiResult<object>> ReadSingleMessage(ReadSingleMessageDto readSingleMessageDto)
        {
            //找到信息并设置已读状态
            var query = _chatInformation.Where(c => c.Id.Equals(readSingleMessageDto.Id));
            var isExist = await query.AnyAsync();
            if (!isExist)
            {
                throw new AbpApiException("没有找到该聊天信息");
            }
            await _chatInformation.SetChatInfReadedAsync(readSingleMessageDto.Id);
            return await ApiResult<object>.GetResultAsync(null);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        /// <exception cref="AbpApiException"></exception>
        [CustomApi(RelationPath = "/ChatManager/SendMsg", Description = "发送信息")]
        public async Task<ApiResult<ChatInformation>> SendMsg(ChatMessageDto chatMessageDto)
        {
            var IsExist = await _userRepository.Where(u => u.Id.Equals(chatMessageDto.ReciveUserId)).AnyAsync();
            if (!IsExist)
            {
                throw new AbpApiException("不存在该用户");
            }
            var user = await _redis.GetAsync<RedisUserDto>(_user.SessionId);
            var send = new ChatInformation
            {
                SenderId = user.Id,
                RecivedId = chatMessageDto.ReciveUserId,
                Message = chatMessageDto.Message,
            };
            send.SetChatInformationId(_guidGenerator.Create());
            await _chatInformation.InsertAsync(send);
            await _abpChatRemoteCall.SendMsgAsync(chatMessageDto.ReciveUserId, chatMessageDto.Message, user, send.Id);
            return await ApiResult<ChatInformation>.GetResultAsync(send);
        }

    }
}
