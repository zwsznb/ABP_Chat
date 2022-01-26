
namespace Custom.Extensions.AuthHelper
{
    public class TokenOptions
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
