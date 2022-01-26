using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Abp.Chat.Demo.Application.Permissions
{
    public class CustomPermissionDefinitionProvider : PermissionDefinitionProvider, ITransientDependency
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("Need_Auth");
            //需要登录的接口全部都要加这个授权，不然到时候会出问题，默认的权限只要需要一个token就可以了，
            //但是我们是根据sessionId去redis中查找的，所以权限不能使用默认的权限
            var needAuthApi = myGroup.AddPermission("Need_Auth_API").WithProviders("UserLogined");
        }
    }
}
