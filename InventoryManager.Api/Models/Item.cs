using System.Collections.Generic;

namespace InventoryManager.Api.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public IDictionary<string, object> Properties { get; set; }
    }
}