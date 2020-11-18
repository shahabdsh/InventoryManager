using System;
using System.Linq;
using InventoryManager.Api.Models;
using InventoryManager.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManager.Api.Services
{
    public abstract class RestrictedRepositoryService<T> : RepositoryService<T>, IRestrictedRepositoryService<T> where T : OwnedEntity
    {
        private readonly RestrictedRepositoryOptions _restrictedRepoOptions;

        protected override FilterDefinition<T> BaseFilterDefinition
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Owner))
                    throw new InvalidOperationException("An owner was not specified.");

                return Builders<T>.Filter.Eq(nameof(OwnedEntity.OwnerId), Owner);
            }
        }

        public string Owner => _restrictedRepoOptions.OwnerId;

        protected RestrictedRepositoryService(IOptions<InventoryDatabaseSettings> dbSettings,
            RestrictedRepositoryOptions restrictedRepoOptions) : base(dbSettings)
        {
            _restrictedRepoOptions = restrictedRepoOptions;
        }

        public override IQueryable<T> Queryable()
        {
            if (string.IsNullOrWhiteSpace(Owner))
                throw new InvalidOperationException("An owner was not specified.");

            return base.Queryable().Where(x => x.OwnerId == Owner);
        }

        public override T Create(T entity)
        {
            if (string.IsNullOrWhiteSpace(Owner))
                throw new InvalidOperationException("An owner was not specified.");

            entity.OwnerId = Owner;

            return base.Create(entity);
        }

        public override void Update(string id, T entityIn)
        {
            if (string.IsNullOrWhiteSpace(Owner))
                throw new InvalidOperationException("An owner was not specified.");

            entityIn.OwnerId = Owner;

            base.Update(id, entityIn);
        }
    }
}
