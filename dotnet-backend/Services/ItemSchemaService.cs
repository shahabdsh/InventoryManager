using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;

namespace InventoryManager.Api.Services
{
    public class ItemSchemaService : RepositoryService<ItemSchema>, IItemSchemaService
    {
        protected override string EntityCollectionName => "ItemSchemas";

        public ItemSchemaService(IOptions<InventoryDatabaseSettings> settings) : base(settings)
        {
        }
    }
}