using System.Collections.Generic;

namespace InventoryManager.Api.Models
{
    public class Item : OwnedEntity
    {
        public string Name { get; set; }
        public string SchemaId { get; set; }
        public int Quantity { get; set; }
        public List<ItemProperty> Properties { get; set; }
    }

    public class ItemProperty
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
