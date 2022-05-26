using EncountifyAPI.Interfaces;
using EncountifyAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EncountifyAPI.Repositories
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        Dictionary<string, string> UsersRecords = new Dictionary<string, string>
        {
            { "user1","password1"},
            { "user2","password2"},
            { "user3","password3"},
        };

        private readonly IConfiguration _configuration;
        private readonly IUserHandlerExecutables _userHandling;
        private readonly string _connectionString;

        public JWTManagerRepository(IConfiguration configuration, IUserHandlerExecutables userHandling)
        {
            _configuration = configuration;
            _userHandling = userHandling;
            _connectionString = configuration.GetConnectionString("EncountifyAPIContext");
        }
        public Token Authenticate(UserLogin users)
        {
            List<User> usersList = _userHandling.ExecuteUserReader(_connectionString, "SELECT * FROM Users");
            if (!usersList.Any(x => x.Email == users.Username && x.Password == users.Password))
            {
                return null;
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                  {
                     new Claim(ClaimTypes.Name, users.Username)
                  }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { CurrentToken = tokenHandler.WriteToken(token) };

        }
    }
}
