using System.Threading.Tasks;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IUserService : IRepositoryService<User>
    {
        User GenerateAndAddGuestUser();
        bool IsTokenRevoked(string token);
        string GenerateJwtTokenFor(string userId);
        Task<User> LoginOrCreateUserWithGoogle(string idToken);
    }
}
