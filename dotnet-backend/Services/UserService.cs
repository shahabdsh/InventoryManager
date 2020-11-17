using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace InventoryManager.Api.Services
{
    public class UserService : RepositoryService<User>, IUserService
    {
        private readonly IConfiguration _configuration;
        protected override string EntityCollectionName => "Users";

        public UserService(IOptions<InventoryDatabaseSettings> settings, IConfiguration configuration) : base(settings)
        {
            _configuration = configuration;
        }

        public User GenerateAndAddGuestUser()
        {
            var userId = ObjectId.GenerateNewId().ToString();

            var user = Create(new User
            {
                Id = userId,
                Tokens = new List<string>
                {
                    GenerateJwtTokenFor(userId)
                }
            });

            return user;
        }

        public string GenerateAndAddJwtTokenFor(string userId)
        {
            var token = GenerateJwtTokenFor(userId);

            var user = GetOne(userId);

            user.Tokens.Add(token);

            return token;
        }

        public bool IsTokenRevoked(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParams = GenerateTokenValidationParameters(_configuration["Authentication:Jwt:Secret"]);

            var principal = tokenHandler.ValidateToken(token, validationParams, out var validatedToken);

            var userId = principal.Claims.Single(claim => claim.Type == nameof(User.Id)).Value;

            var user = GetOne(userId);

            return !user.Tokens.Contains(token);
        }

        private string GenerateJwtTokenFor(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Authentication:Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(nameof(User.Id), userId) }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static TokenValidationParameters GenerateTokenValidationParameters(string key) =>
            new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    }
}
