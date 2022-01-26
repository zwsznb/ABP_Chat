using Abp.Chat.Demo.Application.Hubs;
using Abp.Chat.Demo.Application.Permissions;
using Abp.Chat.Demo.Contract;
using Abp.Chat.Demo.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Abp.Chat.Demo.Application
{
    [DependsOn(typeof(AbpChatDemoContractModule))]
    [DependsOn(typeof(AbpChatDemoEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class AbpChatDemoApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                //Add all mappings defined in the assembly of the MyModule class
                options.AddMaps<AbpChatDemoApplicationModule>();
            });
            //生成guid策略
            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
            });
            Configure<AbpPermissionOptions>(options =>
            {
                options.ValueProviders.Add<UserLoginedPermissionValueProvider>();
            });
            context.Services.AddSingleton<AbpChatRemoteCall>();
        }
    }
}
