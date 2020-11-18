using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Dtos
{
    public class TokenResponse : EntityBase
    {
        public string Token { get; set; }
    }
}
