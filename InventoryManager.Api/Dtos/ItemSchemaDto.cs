using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Dtos
{
    public class ItemSchemaDto : EntityBase
    {
        public string Name { get; set; }
        public ICollection<ItemSchemaProperty> Properties { get; set; }
    }
}