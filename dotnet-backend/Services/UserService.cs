using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using InventoryManager.Api.Models;
using InventoryManager.Api.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManager.Api.Services
{
    public class UserService : RepositoryService<User>, IUserService
    {
        private const string GoogleExternalProviderName = "Google";

        private readonly IOptions<AuthOptions> _authOptions;
        protected override string EntityCollectionName => "Users";

        public UserService(IOptions<InventoryDatabaseOptions> dbSettings,
            IOptions<AuthOptions> authOptions) : base(dbSettings)
        {
            _authOptions = authOptions;
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

            var validationParams =
                GenerateTokenValidationParameters(Environment.GetEnvironmentVariable(EnvironmentVariableNames.JwtSecret));

            var principal = tokenHandler.ValidateToken(token, validationParams, out var validatedToken);

            var userId = principal.Claims.Single(claim => claim.Type == nameof(User.Id)).Value;

            var user = GetOne(userId);

            return user == null || user.RevokedTokens.Contains(token);
        }

        public string GenerateJwtTokenFor(string userId)
        {
            var key = Environment.GetEnvironmentVariable(EnvironmentVariableNames.JwtSecret);

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Google client secret is not initialized");

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(nameof(User.Id), userId) }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _authOptions.Value.GoogleClientId }
            };

            // Todo: Research: Is this secure enough?
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);

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

        public void Logout(string userId, string token)
        {
            var user = GetOne(userId);

            if (!user.RevokedTokens.Contains(token))
                user.RevokedTokens.Add(token);

            Update(user.Id, user);
        }
    }
}
