using Abp.Chat.Demo.Contract.Dto;
using Custom.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Abp.Chat.Demo.Contract.Interfaces
{
    public interface IFriendManagerService : IApplicationService
    {
        Task<ApiResult<AddFriendDto>> AddFriend(InvitationFriendDto invitationDto);
        Task<ApiResult<List<AddFriendDto>>> FindFriends();
        Task<ApiResult<object>> DelFriend(DelFriendDto friendDto);

        Task<ApiResult<object>> UpdateFriendsNickname(UpdateFriendDto friendDto);
    }
}
