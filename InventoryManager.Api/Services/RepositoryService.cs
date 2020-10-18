using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            Get(entity => true);

        public List<T> Get(Expression<Func<T,bool>> filter) =>
            _entities.Find(filter).ToList();

        public List<T> Get(string query)
        {
            return GetInternal(query).ToList();
        }

        public List<string> GetIdsOnly()
        {
            return GetIdsOnly("");
        }

        public List<string> GetIdsOnly(string query)
        {
            return GetInternal(query).Project(entity => entity.Id).ToList();
        }

        private IFindFluent<T, T> GetInternal(string query)
        {
            var filters = ParseQuery(query);

            if (filters.Count > 0)
            {
                var filter = filters[0];
                for (var i = 1; i < filters.Count; i++)
                {
                    filter &= filters[i];
                }

                return _entities.Find(filter);
            }
            else
            {
                return _entities.Find(entity => true);
            }
        }

        public T GetOne(string id) =>
            _entities.Find(entity => entity.Id == id).FirstOrDefault();

        public T Create(T entity)
        {
            entity.CreatedOn = DateTimeOffset.Now;
            _entities.InsertOne(entity);
            return entity;
        }

        public void Update(string id, T entityIn)
        {
            var existing = GetOne(id);

            if (existing == null)
            {
                throw new InvalidOperationException("The entity with the given id does not exist.");
            }

            entityIn.CreatedOn = existing.CreatedOn;
            entityIn.UpdatedOn = DateTimeOffset.Now;

            _entities.ReplaceOne(entity => entity.Id == id, entityIn);
        }

        public void Remove(T entityIn) =>
            _entities.DeleteOne(entity => entity.Id == entityIn.Id);

        public void Remove(string id) =>
            _entities.DeleteOne(entity => entity.Id == id);

        private static List<FilterDefinition<T>> ParseQuery(string query)
        {
            const string querySeparator = ";";
            const string operatorSeparator = "-";
            const string evalSeparator = ":";

            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<FilterDefinition<T>>();
            }

            var builder = Builders<T>.Filter;

            var filters = query.Split(querySeparator).Select(subQuery =>
            {
                var parts = subQuery.Trim().Split(evalSeparator);

                if (parts.Length != 2)
                {
                    throw new Exception($"Each sub-query must only contain one '{evalSeparator}'.");
                }

                var fieldAndOperator = parts[0].Trim();
                var value = parts[1].Trim();

                var op = "eq";
                string field;

                if (fieldAndOperator.Contains(operatorSeparator))
                {
                    var fieldAndOperatorParts = fieldAndOperator.Trim().Split("-");
                    op = fieldAndOperatorParts[1];

                    if (string.IsNullOrWhiteSpace(op))
                    {
                        throw new Exception($"'{operatorSeparator}' must be followed by an operation.");
                    }

                    field = fieldAndOperatorParts[0];
                }
                else
                {
                    field = fieldAndOperator;
                }

                return op switch
                {
                    "eq" => builder.Eq(field, value),
                    "lt" => builder.Lt(field, value),
                    "lte" => builder.Lte(field, value),
                    "gt" => builder.Gt(field, value),
                    "gte" => builder.Gte(field, value),
                    "ctn" => builder.Regex(field, $".*{value}.*"),
                    _ => throw new Exception($"{op} is not a known operation.")
                };
            }).ToList();

            return filters;
        }
    }
}
