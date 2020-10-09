using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IGenericRepositoryService<T> where T : EntityBase
    {
        List<T> Get();
        T Get(string id);
        T Create(T item);
        void Update(string id, T itemIn);
        void Remove(T itemIn);
        void Remove(string id);
    }
}