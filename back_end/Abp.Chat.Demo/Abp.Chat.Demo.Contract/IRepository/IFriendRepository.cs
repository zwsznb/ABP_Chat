using Abp.Chat.Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Abp.Chat.Demo.Contract.IRepository
{
    public interface IFriendRepository : IRepository<Friend, Guid>
    {
    }
}
