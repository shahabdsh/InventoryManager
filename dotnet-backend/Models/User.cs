using System.Collections.Generic;

namespace InventoryManager.Api.Models
{
    public class User : EntityBase
    {
        public List<string> Tokens { get; set; }
    }
}
