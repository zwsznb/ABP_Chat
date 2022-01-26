using Abp.Chat.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Abp.Chat.Demo.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class AbpChatDemoEntityFrameworkCoreContext : AbpDbContext<AbpChatDemoEntityFrameworkCoreContext>
    {
        public AbpChatDemoEntityFrameworkCoreContext(DbContextOptions<AbpChatDemoEntityFrameworkCoreContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<ChatInformation> ChatInformation { get; set; }
        public DbSet<Invitation> Invitation { get; set; }
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
