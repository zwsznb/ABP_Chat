using Abp.Chat.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations
{
    [ConnectionStringName("Default")]
    public class AbpChatDemoEntityFrameworkMigrationContext : AbpDbContext<AbpChatDemoEntityFrameworkMigrationContext>
    {
        public AbpChatDemoEntityFrameworkMigrationContext(DbContextOptions<AbpChatDemoEntityFrameworkMigrationContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Friend>().ToTable("Friend");
            modelBuilder.Entity<ChatInformation>().ToTable("ChatInformation");
            modelBuilder.Entity<Invitation>().ToTable("Invitation");
        }
    }
}
