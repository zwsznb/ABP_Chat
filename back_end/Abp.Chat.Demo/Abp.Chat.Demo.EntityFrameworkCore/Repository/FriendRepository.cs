using Abp.Chat.Demo.Contract.IRepository;
using Abp.Chat.Demo.Domain;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Abp.Chat.Demo.EntityFrameworkCore.Repository
{
    public class FriendRepository : EfCoreRepository<AbpChatDemoEntityFrameworkCoreContext, Friend, Guid>, IFriendRepository
    {
        public FriendRepository(IDbContextProvider<AbpChatDemoEntityFrameworkCoreContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
