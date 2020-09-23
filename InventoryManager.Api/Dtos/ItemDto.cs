using System.Collections.Generic;

namespace InventoryManager.Api.Dtos
{
    public class ItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}