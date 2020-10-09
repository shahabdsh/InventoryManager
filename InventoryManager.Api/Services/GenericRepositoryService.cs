using System;
using System.Collections.Generic;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManager.Api.Services
{
    public abstract class GenericRepositoryService<T> where T : EntityBase
    {
        private readonly IMongoCollection<T> _items;
        
        protected abstract string ItemCollectionName { get; }

        protected GenericRepositoryService(IOptions<InventoryDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _items = database.GetCollection<T>(ItemCollectionName);
        }

        public List<T> Get() =>
            _items.Find(item => true).ToList();

        public T Get(string id) =>
            _items.Find<T>(item => item.Id == id).FirstOrDefault();

        public T Create(T item)
        {
            _items.InsertOne(item);
            return item;
        }

        public void Update(string id, T itemIn)
        {
            _items.ReplaceOne(item => item.Id == id, itemIn);
        }

        public void Remove(T itemIn) =>
            _items.DeleteOne(item => item.Id == itemIn.Id);

        public void Remove(string id) => 
            _items.DeleteOne(item => item.Id == id);
    }
}