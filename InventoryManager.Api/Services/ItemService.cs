using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;

namespace InventoryManager.Api.Services
{
    public class ItemService : GenericRepositoryService<Item>, IItemService
    {
        protected override string ItemCollectionName => "Items";
        
        public ItemService(IOptions<InventoryDatabaseSettings> settings) : base(settings)
        {
        }
    }
}