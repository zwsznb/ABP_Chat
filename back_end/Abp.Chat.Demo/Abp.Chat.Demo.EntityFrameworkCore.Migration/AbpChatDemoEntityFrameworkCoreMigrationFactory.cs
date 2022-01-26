using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations
{
    public class AbpChatDemoEntityFrameworkCoreMigrationFactory : IDesignTimeDbContextFactory<AbpChatDemoEntityFrameworkMigrationContext>
    {
        public AbpChatDemoEntityFrameworkMigrationContext CreateDbContext(string[] args)
        {

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AbpChatDemoEntityFrameworkMigrationContext>()
                .UseMySql(configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(configuration.GetConnectionString("Default")));

            return new AbpChatDemoEntityFrameworkMigrationContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
