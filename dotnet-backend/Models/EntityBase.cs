using System;

namespace InventoryManager.Api.Models
{
    public class EntityBase
    {
        public string Id { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}