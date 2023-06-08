    using MedicaBE.Entities.Auth;
    using MedicaBE.Entities.Models;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;

    namespace MedicaBE.Auth
    {
        public class JwtTokenHelper
        {
           private readonly IConfiguration _configuration;
            public JwtTokenHelper(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public string GenerateToken(JwtSetting jwtSetting, Retailer user)
            {
                if (jwtSetting == null)
                    return string.Empty;

            var token1Key = _configuration.GetValue<string>("JwtSetting:Token1:Key");
            jwtSetting.Issuer = _configuration.GetValue<string>("JwtSetting:Token1:Issuer");
            jwtSetting.Audience = _configuration.GetValue<string>("JwtSetting:Token1:Audience");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token1Key));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("CustomClaimForUser", JsonSerializer.Serialize(user)) // Additional Claims
                };

                var token = new JwtSecurityToken(
                jwtSetting.Issuer,
                jwtSetting.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60), // Default 5 mins, max 1 day
                signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

             public string GenerateUserToken(JwtSetting jwtSetting, User user)
            {
                if (jwtSetting == null)
                    return string.Empty;

                var token2Key = _configuration.GetValue<string>("JwtSetting:Token2:Key");
                jwtSetting.Issuer = _configuration.GetValue<string>("JwtSetting:Token2:Issuer");
                jwtSetting.Audience = _configuration.GetValue<string>("JwtSetting:Token2:Audience");
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token2Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim("CustomClaimForUser", JsonSerializer.Serialize(user)) // Additional Claims
                };

                var token = new JwtSecurityToken(
                    jwtSetting.Issuer,
                    jwtSetting.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60), // Default 5 mins, max 1 day
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
