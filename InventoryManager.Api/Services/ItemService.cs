using System.Linq;
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

        protected override IQueryable<Item> SimpleQueryFilter(string query)
        {
            return Entities.Where(entity =>
                entity.Name.ToLower().Contains(query.ToLower()) ||
                entity.Properties.Any(prop => prop.Value.ToLower().Contains(query.ToLower())));
        }

        protected override IQueryable<Item> AdvancedQueryFilter(string query)
        {
            return Entities;
        }
    }
}
