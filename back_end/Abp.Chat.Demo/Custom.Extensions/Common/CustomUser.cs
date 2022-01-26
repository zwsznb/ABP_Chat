using System.Security.Claims;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Custom.Extensions.Common
{
    /// <summary>
    /// 重写认证用户凭证，把sessionId添加进去,
    /// 实际上在认证的时候就可以加进去，但是abp有这功能就用上了吧
    /// </summary>
    public class CustomUser : CurrentUser, ICustomUser
    {
        private readonly ClaimsPrincipal _principal;
        public CustomUser(ICurrentPrincipalAccessor principalAccessor) : base(principalAccessor)
        {
            _principal = principalAccessor.Principal;
        }
        public string SessionId => _principal.FindFirstValue("SessionId");
    }
    public interface ICustomUser
    {
        public string SessionId { get; }
    }
}
