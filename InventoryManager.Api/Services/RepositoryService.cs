using System;
using System.Collections.Generic;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManager.Api.Services
{
    public abstract class RepositoryService<T> : IRepositoryService<T> where T : EntityBase
    {
        private readonly IMongoCollection<T> _entities;
        
        protected abstract string EntityCollectionName { get; }

        protected RepositoryService(IOptions<InventoryDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _entities = database.GetCollection<T>(EntityCollectionName);
        }

        public List<T> Get() =>
            _entities.Find(entity => true).ToList();

        public T Get(string id) =>
            _entities.Find<T>(entity => entity.Id == id).FirstOrDefault();

        public T Create(T entity)
        {
            _entities.InsertOne(entity);
            return entity;
        }

        public void Update(string id, T entityIn)
        {
            _entities.ReplaceOne(entity => entity.Id == id, entityIn);
        }

        public void Remove(T entityIn) =>
            _entities.DeleteOne(entity => entity.Id == entityIn.Id);

        public void Remove(string id) => 
            _entities.DeleteOne(entity => entity.Id == id);
    }
}