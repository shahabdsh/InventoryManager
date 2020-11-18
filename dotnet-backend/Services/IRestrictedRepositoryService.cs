using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IRestrictedRepositoryService<T> : IRepositoryService<T> where T : EntityBase
    {
    }
}
