using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace InventoryManager.Api.Services
{
    public class UserService : RepositoryService<User>, IUserService
    {
        private const string GoogleExternalProviderName = "Google";

        private readonly IConfiguration _configuration;
        protected override string EntityCollectionName => "Users";

        public UserService(IOptions<InventoryDatabaseSettings> dbSettings, IConfiguration configuration) : base(dbSettings)
        {
            _configuration = configuration;
        }

        public User GenerateAndAddGuestUser()
        {

            var user = Create(new User());

            return user;
        }

        public async Task<User> LoginOrCreateUserWithGoogle(string idToken)
        {
            var payload = await ValidateGoogleToken(idToken);

            if (payload != null)
            {
                var user = Queryable().SingleOrDefault(x => x.ExternalProvider == GoogleExternalProviderName &&
                                                  x.ExternalId == payload.Email);

                if (user == null)
                {
                    user = Create(new User
                    {
                        ExternalProvider = GoogleExternalProviderName,
                        ExternalId = payload.Email
                    });
                }

                return user;
            }

            throw new ArgumentException($"The given id token is invalid.");
        }

        public bool IsTokenRevoked(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParams = GenerateTokenValidationParameters(_configuration["Authentication:Jwt:Secret"]);

            var principal = tokenHandler.ValidateToken(token, validationParams, out var validatedToken);

            var userId = principal.Claims.Single(claim => claim.Type == nameof(User.Id)).Value;

            var user = GetOne(userId);

            return user == null || user.RevokedTokens.Contains(token);
        }

        public string GenerateJwtTokenFor(string userId)
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

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
                new GoogleJsonWebSignature.ValidationSettings());

            return payload;
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
