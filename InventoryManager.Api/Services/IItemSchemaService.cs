using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IItemSchemaService
    {
        List<ItemSchema> Get();
        ItemSchema Get(string id);
        ItemSchema Create(ItemSchema itemSchema);
        void Update(string id, ItemSchema itemSchemaIn);
        void Remove(ItemSchema itemSchemaIn);
        void Remove(string id);
    }
}