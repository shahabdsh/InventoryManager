using System;

namespace InventoryManager.Api.Models
{
    public class OwnedEntity : EntityBase
    {
        public string OwnerId { get; set; }
    }
}
