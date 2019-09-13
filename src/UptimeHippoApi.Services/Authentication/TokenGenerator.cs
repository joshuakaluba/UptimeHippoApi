using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Authentication;
using TokenOptions = UptimeHippoApi.Data.Models.Authentication.TokenOptions;

namespace UptimeHippoApi.Services.Authentication
{
    public static class TokenGenerator
    {
        public static async Task<Token> CreateJwtToken(ApplicationUser user, UserManager<ApplicationUser> userManager, TokenOptions tokenOptions)
        {
            var userClaims = await userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryDate = DateTime.Now.AddYears(3);

            var jwsSecurityToken = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Issuer,
                claims: userClaims,
                expires: expiryDate,
                signingCredentials: credentials);

            return new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwsSecurityToken),
                Email = user.Email,
                ExpiryDate = expiryDate
            };
        }
    }
}