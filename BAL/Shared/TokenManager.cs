using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Model;

namespace WebAPI.BAL
{
    public class TokenManager
    {
        private AppSetting _appSetting;

        public TokenManager(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }
        public string GenerateToken(Token request)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("Mobile", request.Mobile),
                    new Claim("Role", request.Role.ToString())
                    }),
                    Issuer = _appSetting.Issuer,
                    Audience = _appSetting.Audience,
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Token GetClaimToken(HttpContext context)
        {
            Token claim = new Token();
            foreach (var item in context.User.Claims)
            {
                if (item.Type == "Mobile")
                {
                    claim.Mobile = item.Value;
                }
                else if (item.Type == "Role")
                {
                    claim.Role = int.Parse(item.Value);
                }
            }
            return claim;
        }
    }
}
