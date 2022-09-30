using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        // We need constructor because we're going to inject configuration into this class.
        // We pass UserManager because this is how we get the UserRole.
        public TokenService(IConfiguration config)
        {
            _config = config;
            // We're also going to need to create a symmetric security key so that we can sign the token.
            // SymmetricSecurityKey take byte array of the key, and we need to encode it because it created a
            // key as a string, and we need to encode it into a byte array.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            { 
                // Add new claim setting the email to the user's email.
                // It's beneficial to have the token be as small as possible because you're sending this token up with every single request
                // and it's going to be encrypted inside the header and we're going to be storing the token on the client browser. So if a user really wanted to,
                // they could see what information you're holding inside that token.
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName),
            };

            // Create new credentials.
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Describe token, we put the information we want in payload.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Token:Key"]
            };

            // Create token handler.
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // This is going to be in the formats that we looked at in JWT.io and signed appropriately by our server as well.
            return tokenHandler.WriteToken(token);
        }
    }
}
