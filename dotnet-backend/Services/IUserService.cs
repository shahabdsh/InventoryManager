using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IUserService : IRepositoryService<User>
    {
        User GenerateAndAddGuestUser();
        string GenerateAndAddJwtTokenFor(string userId);
        bool IsTokenRevoked(string token);
    }
}
