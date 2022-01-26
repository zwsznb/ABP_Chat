using Abp.Chat.Demo.Domain;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Abp.Chat.Demo.Contract.IRepository
{
    public interface IInvitationRepository : IRepository<Invitation, Guid>
    {
        Task SetInvitationReadedAsync(Guid userId);
        Task AggreeInvitationAsync(Guid InvitationId);
    }
}
