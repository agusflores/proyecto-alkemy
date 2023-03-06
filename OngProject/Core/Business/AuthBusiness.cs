using Microsoft.IdentityModel.Tokens;
using OngProject.Core.Interfaces;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace OngProject.Core.Business
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IConfiguration _config;

        public AuthBusiness(IConfiguration config)
        {
            _config = config;
        }
        public string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };

            var authSigningKey = new SymmetricSecurityKey(key);

            var token = new JwtSecurityToken
            (
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return tokenHandler.WriteToken(token);
        }
    }
}
