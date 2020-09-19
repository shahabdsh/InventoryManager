using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IItemService
    {
        List<Item> Get();
        Item Get(string id);
        Item Create(Item item);
        void Update(string id, Item itemIn);
        void Remove(Item itemIn);
        void Remove(string id);
    }
}