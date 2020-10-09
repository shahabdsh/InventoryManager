using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;

namespace InventoryManager.Api.Services
{
    public class ItemService : RepositoryService<Item>, IItemService
    {
        protected override string EntityCollectionName => "Items";
        
        public ItemService(IOptions<InventoryDatabaseSettings> settings) : base(settings)
        {
        }
    }
}