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
    public class ItemService : IItemService
    {
        private readonly IMongoCollection<Item> _items;

        public ItemService(IOptions<InventoryDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _items = database.GetCollection<Item>(settings.Value.ItemsCollectionName);
        }

        public List<Item> Get() =>
            _items.Find(book => true).ToList();

        public Item Get(string id) =>
            _items.Find<Item>(book => book.Id == id).FirstOrDefault();

        public Item Create(Item item)
        {
            _items.InsertOne(item);
            return item;
        }

        public void Update(string id, Item itemIn)
        {
            _items.ReplaceOne(book => book.Id == id, itemIn);
        }
            

        public void Remove(Item itemIn) =>
            _items.DeleteOne(book => book.Id == itemIn.Id);

        public void Remove(string id) => 
            _items.DeleteOne(book => book.Id == id);
    }
}