using System.Collections.Generic;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IRepositoryService<T> where T : EntityBase
    {
        List<T> Get();
        T Get(string id);
        T Create(T entity);
        void Update(string id, T entityIn);
        void Remove(T entityIn);
        void Remove(string id);
    }
}