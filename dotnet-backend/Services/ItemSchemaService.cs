using InventoryManager.Api.Models;
using InventoryManager.Api.Options;
using Microsoft.Extensions.Options;

namespace InventoryManager.Api.Services
{
    public class ItemSchemaService : RestrictedRepositoryService<ItemSchema>, IItemSchemaService
    {
        protected override string EntityCollectionName => "ItemSchemas";

        public ItemSchemaService(IOptions<InventoryDatabaseOptions> dbSettings,
            UserContext restrictedRepoOptions) : base(dbSettings, restrictedRepoOptions)
        {
        }
    }
}
