using Abp.Chat.Demo.Contract.IRepository;
using Abp.Chat.Demo.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Abp.Chat.Demo.EntityFrameworkCore.Repository
{
    public class InvitationRepository : EfCoreRepository<AbpChatDemoEntityFrameworkCoreContext, Invitation, Guid>, IInvitationRepository
    {
        public InvitationRepository(IDbContextProvider<AbpChatDemoEntityFrameworkCoreContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        //设置邀请已经读取
        public async Task SetInvitationReadedAsync(Guid userId)
        {
            var Db = await GetDbContextAsync();
            await Db.Database.ExecuteSqlRawAsync(
                $"update Invitation set isReaded=1 where chumId='{userId}' and isReaded=0;"
            );
        }
        public async Task AggreeInvitationAsync(Guid InvitationId)
        {
            var Db = await GetDbContextAsync();
            await Db.Database.ExecuteSqlRawAsync(
                $"update Invitation set isAccept=1 where Id='{InvitationId}';"
            );
        }
    }
}
