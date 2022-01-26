using Abp.Chat.Demo.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Abp.Chat.Demo.EntityFrameworkCore
{
    [DependsOn(typeof(AbpChatDemoDomainModule))]
    public class AbpChatDemoEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpChatDemoEntityFrameworkCoreContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL<AbpChatDemoEntityFrameworkCoreContext>();
            });
        }
    }
}
