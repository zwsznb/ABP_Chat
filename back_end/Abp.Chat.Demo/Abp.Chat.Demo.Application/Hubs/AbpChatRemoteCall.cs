using Abp.Chat.Demo.Contract.Dto;
using Abp.Chat.Demo.Domain.Share;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Abp.Chat.Demo.Application.Hubs
{
    public class AbpChatRemoteCall : ISingletonDependency
    {
        private readonly IHubContext<AbpChatHub> _hubContext;
        private readonly IRedisDatabase _redis;
        public AbpChatRemoteCall(IHubContext<AbpChatHub> hubContext,
            IRedisDatabase redis)
        {
            _hubContext = hubContext;
            _redis = redis;
        }
        //添加邀请
        public async Task AddInvitaionAsync(Guid userId)
        {
            RedisConnectIdDicDto redisConnectIdDic = await GetRedisConnectionIdDic(userId);
            //发没发成功不用管
            await _hubContext.Clients.Client(redisConnectIdDic?.ConnectionId ?? "").SendAsync("AddInvitation", "Success");
        }

        public async Task TestCallAsync()
        {
            //await _hubContext.Clients.All.SendAsync("test", "test");
            await _hubContext.Clients.Client("ddddd").SendAsync("test", "test");
        }

        //添加好友
        public async Task AddNewFriendAsync(Guid friendId, RedisUserDto user)
        {
            RedisConnectIdDicDto redisConnectIdDic = await GetRedisConnectionIdDic(friendId);
            //发没发成功不用管
            await _hubContext.Clients.Client(redisConnectIdDic?.ConnectionId ?? "").SendAsync("AddNewFriend", JsonConvert.SerializeObject(user));
        }
        private async Task<RedisConnectIdDicDto> GetRedisConnectionIdDic(Guid userId)
        {
            return await _redis.GetAsync<RedisConnectIdDicDto>(HubConst.RedisConnectionIdPrefix + userId);
        }

        public async Task SendMsgAsync(Guid reciveUserId, string message, RedisUserDto user, Guid messageId)
        {
            RedisConnectIdDicDto redisConnectIdDic = await GetRedisConnectionIdDic(reciveUserId);
            await SendMessageAsync(message, redisConnectIdDic, messageId, user);
            await SendNoReadCountAsync(redisConnectIdDic, user);
        }

        private async Task SendNoReadCountAsync(RedisConnectIdDicDto redisConnectIdDic, RedisUserDto user)
        {
            //发没发成功不用管
            await _hubContext.Clients.Client(redisConnectIdDic?.ConnectionId ?? "").SendAsync("SendNoReadCount", user.Id);
        }

        private async Task SendMessageAsync(string message, RedisConnectIdDicDto redisConnectIdDic, Guid messageId, RedisUserDto user)
        {
            //发没发成功不用管
            await _hubContext.Clients.Client(redisConnectIdDic?.ConnectionId ?? "").SendAsync("SendMsg", JsonConvert.SerializeObject(new { message = message, messageId = messageId, userId = user.Id }));
        }
    }
}
