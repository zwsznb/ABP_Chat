using Abp.Chat.Demo.Contract.Dto;
using Abp.Chat.Demo.Domain.Share;
using Custom.Extensions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace Abp.Chat.Demo.Application.Hubs
{
    /// <summary>
    /// 这里的授权是通过查询字符串access_token进行传递token的
    /// 先以UserId+ConnectionId作为key存redis，生存期定跟token一样
    /// 这里应该尽可能的不放业务逻辑，仅当一个总线使用
    /// </summary>
    [Authorize("Need_Auth_API")]
    [HubRoute("/abpChatHub")]
    public class AbpChatHub : Hub
    {
        private readonly IRedisDatabase _redis;
        private readonly ICustomUser _user;
        public AbpChatHub(IRedisDatabase redis, ICustomUser user)
        {
            _redis = redis;
            _user = user;
        }
        //用户上线
        public override Task OnConnectedAsync()
        {
            var redisUser = _redis.GetAsync<RedisUserDto>(_user.SessionId).Result;
            //当上线时在redis中添加connectionId映射
            //先去查找存不存在映射
            var connectId = HubConst.RedisConnectionIdPrefix + redisUser.Id;
            var connect = _redis.GetAsync<RedisConnectIdDicDto>(connectId).Result;
            //如果不存在连接
            if (connect != null)
            {
                _redis.RemoveAsync(connectId);
            }
            //同样设置三十秒过期
            _redis.AddAsync(connectId, new RedisConnectIdDicDto { ConnectionId = Context.ConnectionId }, TimeSpan.FromMinutes(30));
            return base.OnConnectedAsync();
        }
        public async Task Hearbeat(string msg)
        {
            var redisUser = _redis.GetAsync<RedisUserDto>(_user.SessionId).Result;
            string connectId = "";
            connectId = HubConst.RedisConnectionIdPrefix + redisUser?.Id ?? "";
            var connect = await _redis.GetAsync<RedisConnectIdDicDto>(connectId);
            await Clients.Client(connect?.ConnectionId ?? "").SendAsync("heartBeat", msg);
        }
    }
}
