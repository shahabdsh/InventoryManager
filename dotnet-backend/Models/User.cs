using System.Collections.Generic;

namespace InventoryManager.Api.Models
{
    public class User : EntityBase
    {
        public List<string> RevokedTokens { get; set; }
        public string ExternalProvider { get; set; }
        public string ExternalId { get; set; }

        public User()
        {
            RevokedTokens = new List<string>();
        }
    }
}
