using MedicaBE.Entities.Models;
using MedicaBE.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace MedicaBE.Repository.Repository
{
    public class JwtTokensRepository : IJwtTokensRepository
    {
        private readonly IConfiguration _configuration;
        public JwtTokensRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateTokenForUser(User user)
        {
           // if (jwtsettings == null)
           //     return string.Empty;

           //// ApplicationUser jwtsettings,


           // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.Key));
           // var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

           // var claimss = new[]
           // {
           //     new Claim(ClaimTypes.Name, user.FirstName),
           //     new Claim("CustomClaimForUser", JsonSerializer.Serialize(user))
           // };

           // var tokens = new JwtSecurityToken(
           //     jwtsettings.Issuer,
           //     jwtsettings.Audience,
           //     claimss,
           //     expires: DateTime.UtcNow.AddMinutes(60),
           //     signingCredentials: credential);





            List<Claim> claims = new List<Claim>
            {
               // new Claim(ClaimTypes.Name, user.FirstName),
                new Claim("CustomClaimForUser", JsonSerializer.Serialize(user))
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                _configuration.GetSection("AppSettings:Issuer").Value,
                _configuration.GetSection("AppSettings:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}










