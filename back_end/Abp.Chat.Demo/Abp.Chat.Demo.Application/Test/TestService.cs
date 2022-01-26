using Abp.Chat.Demo.Application.Hubs;
using Custom.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Abp.Chat.Demo.Application.Test
{
    [ApiExplorerSettings(GroupName = "测试服务")]
    //[Authorize]
    public class TestService : ApplicationService
    {
        private readonly AbpChatRemoteCall _abpChatRemoteCall;
        public TestService(IRedisDatabase redis, AbpChatRemoteCall abpChatRemoteCall)
        {
            _abpChatRemoteCall = abpChatRemoteCall;
            redis.AddAsync("test", new { message = "ddd", data = "gggggg" }, TimeSpan.FromSeconds(60));
        }
        [CustomApi(RelationPath = "/GetTestData", Description = "获取测试数据", Method = "Get")]
        public async Task<ApiResult<object>> TestAsync()
        {
            await _abpChatRemoteCall.TestCallAsync();
            return ApiResult<object>.GetResult(new { message = "test" });
        }
    }
}
