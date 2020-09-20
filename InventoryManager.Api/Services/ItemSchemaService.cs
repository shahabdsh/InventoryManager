using System;
using System.Collections.Generic;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace InventoryManager.Api.Services
{
    public class ItemSchemaService : IItemSchemaService
    {
        private readonly IMongoCollection<ItemSchema> _itemSchemas;

        public ItemSchemaService(IOptions<InventoryDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _itemSchemas = database.GetCollection<ItemSchema>(settings.Value.ItemSchemaCollectionName);
        }

        public List<ItemSchema> Get() =>
            _itemSchemas.Find(itemSchema => true).ToList();

        public ItemSchema Get(string id) =>
            _itemSchemas.Find<ItemSchema>(itemSchema => itemSchema.Id == id).FirstOrDefault();

        public ItemSchema Create(ItemSchema itemSchema)
        {
            _itemSchemas.InsertOne(itemSchema);
            return itemSchema;
        }

        public void Update(string id, ItemSchema itemSchemaIn)
        {
            _itemSchemas.ReplaceOne(itemSchema => itemSchema.Id == id, itemSchemaIn);
        }
            

        public void Remove(ItemSchema itemSchemaIn) =>
            _itemSchemas.DeleteOne(itemSchema => itemSchema.Id == itemSchemaIn.Id);

        public void Remove(string id) => 
            _itemSchemas.DeleteOne(itemSchema => itemSchema.Id == id);
    }
}