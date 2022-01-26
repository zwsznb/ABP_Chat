using Abp.Chat.Demo.Contract.IRepository;
using Abp.Chat.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Abp.Chat.Demo.EntityFrameworkCore.Repository
{
    public class ChatInformationRepository : EfCoreRepository<AbpChatDemoEntityFrameworkCoreContext, ChatInformation, Guid>, IChatInformationRepository
    {
        public ChatInformationRepository(IDbContextProvider<AbpChatDemoEntityFrameworkCoreContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task SetChatInfReadedAsync(Guid infId)
        {
            var Db = await GetDbContextAsync();
            await Db.Database.ExecuteSqlRawAsync(
                $"update ChatInformation set isRead=1 where Id='{infId}' and isRead=0;"
            );
        }
    }
}
