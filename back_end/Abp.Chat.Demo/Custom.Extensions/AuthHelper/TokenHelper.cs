using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Custom.Extensions.AuthHelper
{
    public class TokenHelper
    {
        private readonly IOptions<TokenOptions> _options;
        public TokenHelper(IOptions<TokenOptions> options)
        {
            _options = options;
        }
        public string TokenCreate(Dictionary<string, string> claimDic)
        {
            List<Claim> claims = new List<Claim>();
            foreach (var dic in claimDic)
            {
                claims.Add(new Claim(dic.Key, dic.Value));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
