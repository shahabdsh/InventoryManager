using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Dtos
{
    public class ItemDto : EntityBase
    {
        public string Name { get; set; }
        public string SchemaId { get; set; }
        public int Quantity { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}