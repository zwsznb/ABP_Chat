using Abp.Chat.Demo.Domain;
using System;
using Volo.Abp.Domain.Repositories;

namespace Abp.Chat.Demo.Contract.IRepository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
