using Abp.Chat.Demo.Contract.Dto;
using Custom.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Abp.Chat.Demo.Contract.Interfaces
{
    public interface IAccountService : IApplicationService
    {
        Task<ApiResult<object>> Register(UserDto user);
        Task<ApiResult<object>> Login(UserDto user);
        Task<ApiResult<object>> Loginout();
        Task<ApiResult<List<RedisUserDto>>> FindUsers(string searchStr);
        Task<ApiResult<object>> ChangePass(ChangePasswordDto changePasswordDto);
    }
}
