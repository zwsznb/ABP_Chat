using Abp.Chat.Demo.Contract.Dto;
using Custom.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Abp.Chat.Demo.Contract.Interfaces
{
    public interface IFriendInvitationService : IApplicationService
    {
        Task<ApiResult<object>> AddInvitaion(InvitationDto invitationDto);
        Task<ApiResult<object>> ReadAllInvitation();
        Task<ApiResult<List<FriendInvitationDto>>> FindAllInvitation();
        Task<ApiResult<int>> FindNewInvitation();
        Task<ApiResult<object>> RefusedInvitation(RefusedInvitationDto invitationDto);
        Task<ApiResult<object>> DelInvitation(RefusedInvitationDto invitationDto);
    }
}
