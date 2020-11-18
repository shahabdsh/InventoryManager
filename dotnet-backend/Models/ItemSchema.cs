using System.Collections.Generic;

namespace InventoryManager.Api.Models
{
    public class ItemSchema : OwnedEntity
    {
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
        Number = 1,
        Checkbox = 2
    }
}
