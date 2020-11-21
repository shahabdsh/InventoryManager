using InventoryManager.Api.Models;

namespace InventoryManager.Api.Dtos
{
    public class LoginResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
