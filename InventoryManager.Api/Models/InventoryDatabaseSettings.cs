namespace InventoryManager.Api.Models
{
    public class InventoryDatabaseSettings
    {
        public string ItemCollectionName { get; set; }
        public string ItemSchemaCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}