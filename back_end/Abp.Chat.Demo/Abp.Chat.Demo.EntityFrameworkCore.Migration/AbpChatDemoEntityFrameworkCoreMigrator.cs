using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations
{
    public class AbpChatDemoEntityFrameworkCoreMigrator : ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public AbpChatDemoEntityFrameworkCoreMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the AngularMaterialMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<AbpChatDemoEntityFrameworkMigrationContext>()
                .Database
                .MigrateAsync();
        }
    }
}
