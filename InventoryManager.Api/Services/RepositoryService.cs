using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using InventoryManager.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManager.Api.Services
{
    public abstract class RepositoryService<T> : IRepositoryService<T> where T : EntityBase
    {
        private readonly IMongoCollection<T> _entities;

        protected abstract string EntityCollectionName { get; }

        protected IQueryable<T> Entities => _entities.AsQueryable();

        protected RepositoryService(IOptions<InventoryDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _entities = database.GetCollection<T>(EntityCollectionName);
        }

        public List<T> Get() =>
            Get(entity => true);

        public List<T> Get(Expression<Func<T, bool>> filter) =>
            _entities.Find(filter).ToList();

        public List<T> Get(string query)
        {
            return GetQueryInternal(query).ToList();
        }

        public List<string> GetIdsOnly()
        {
            return GetIdsOnly("");
        }

        public List<string> GetIdsOnly(string query)
        {
            return GetQueryInternal(query).Select(entity => entity.Id).ToList();
        }

        private IQueryable<T> GetQueryInternal(string query)
        {
            var complexQueryRegex = new Regex(@"{\s*(.*)\s*}");
            var match = complexQueryRegex.Match(query);

            return match.Success
                ? AdvancedQueryFilter(match.Groups[1].Value)
                : SimpleQueryFilter(query);
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

        protected virtual IQueryable<T> AdvancedQueryFilter(string query)
        {
            return _entities.AsQueryable().Where(entity => true);
        }

        protected virtual IQueryable<T> SimpleQueryFilter(string query)
        {
            return _entities.AsQueryable().Where(entity => true);
        }

        protected List<BasicFilterDefinition> ParseQuery(string query)
        {
            const string querySeparator = ";";
            const string operatorSeparator = "-";
            const string evalSeparator = ":";

            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<BasicFilterDefinition>();
            }

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
                    op = fieldAndOperatorParts[1].Trim();

                    if (string.IsNullOrWhiteSpace(op))
                    {
                        throw new Exception($"'{operatorSeparator}' must be followed by an operation.");
                    }

                    field = fieldAndOperatorParts[0].Trim();
                }
                else
                {
                    field = fieldAndOperator;
                }

                return new BasicFilterDefinition
                {
                    Field = field,
                    Value = value,
                    Operation = op switch
                    {
                        "eq" => BasicFilterOperation.Eq,
                        "lt" => BasicFilterOperation.Lt,
                        "lte" => BasicFilterOperation.Lte,
                        "gt" => BasicFilterOperation.Gt,
                        "gte" => BasicFilterOperation.Gte,
                        "ctn" => BasicFilterOperation.Ctn,
                        _ => throw new Exception($"{op} is not a known operation.")
                    }
                };

            }).ToList();

            return filters;
        }
    }

    public class BasicFilterDefinition
    {
        public string Field { get; set; }
        public BasicFilterOperation Operation { get; set; }
        public string Value { get; set; }
    }

    public enum BasicFilterOperation
    {
        Eq,
        Lt,
        Lte,
        Gt,
        Gte,
        Ctn
    }
}
