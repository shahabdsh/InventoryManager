using System.Collections.Generic;

namespace InventoryManager.Api.Models
{
    public class ItemSchema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<ItemSchemaProperty> Properties { get; set; }
    }

    public class ItemSchemaProperty
    {
        public string Name { get; set; }
        public ItemSchemaPropertyType Type { get; set; }
    }

    public enum ItemSchemaPropertyType
    {
        Text = 0,
        Number = 1
    }
}