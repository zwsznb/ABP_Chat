using Abp.Chat.Demo.Contract.Dto;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Abp.Chat.Demo.Application.Permissions
{
    /// <summary>
    /// 这个权限用来判断当前用户已经登录，登录的判断依据是redis中存在缓存
    /// 这里就不使用store了，直接在这里判断权限
    /// </summary>
    public class UserLoginedPermissionValueProvider : PermissionValueProvider
    {
        private readonly IRedisDatabase _redis;
        public UserLoginedPermissionValueProvider(IPermissionStore permissionStore,
            IRedisDatabase redis) : base(permissionStore)
        {
            _redis = redis;
        }

        public override string Name => "UserLogined";

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            //拿到凭证中sessionId去redis中查找
            var sessionId = context.Principal.FindFirst("SessionId");
            if (sessionId == null)
            {
                return await Task.FromResult(PermissionGrantResult.Undefined);
            }
            //去reids中查找用户
            var reidsUser = await _redis.GetAsync<RedisUserDto>(sessionId.Value);
            if (reidsUser == null)
            {
                return await Task.FromResult(PermissionGrantResult.Undefined);
            }
            return await Task.FromResult(PermissionGrantResult.Granted);
        }
        /// <summary>
        /// 这个正常来说应该是用不到的，因为在源码中没有看到相应的实现模板，只能自己实现
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            throw new NotImplementedException();
        }
    }
}
