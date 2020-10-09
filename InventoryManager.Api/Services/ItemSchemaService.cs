using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;

namespace InventoryManager.Api.Services
{
    public class ItemSchemaService : GenericRepositoryService<ItemSchema>, IItemSchemaService
    {
        protected override string ItemCollectionName => "ItemSchemas";

        public ItemSchemaService(IOptions<InventoryDatabaseSettings> settings) : base(settings)
        {
        }
    }
}