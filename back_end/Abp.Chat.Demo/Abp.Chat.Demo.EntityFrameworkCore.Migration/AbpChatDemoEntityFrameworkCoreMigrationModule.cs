using Abp.Chat.Demo.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpChatDemoEntityFrameworkCoreMigrationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpChatDemoEntityFrameworkMigrationContext>();
        }
    }
}
