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
        private InvalidOperationException UserNotSpecifiedException =>
            new InvalidOperationException("A user was not specified.");

        private readonly UserContext _restrictedRepoOptions;

        protected override FilterDefinition<T> BaseFilterDefinition
        {
            get
            {
                AssertUserSpecified();

                return Builders<T>.Filter.Eq(nameof(OwnedEntity.OwnerId), UserId);
            }
        }

        public string UserId => _restrictedRepoOptions.UserId;

        protected RestrictedRepositoryService(IOptions<InventoryDatabaseOptions> dbSettings,
            UserContext restrictedRepoOptions) : base(dbSettings)
        {
            _restrictedRepoOptions = restrictedRepoOptions;
        }

        public override IQueryable<T> Queryable()
        {
            AssertUserSpecified();

            return base.Queryable().Where(x => x.OwnerId == UserId);
        }

        public override T Create(T entity)
        {
            AssertUserSpecified();

            entity.OwnerId = UserId;

            return base.Create(entity);
        }

        public override void Update(string id, T entityIn)
        {
            AssertUserSpecified();

            entityIn.OwnerId = UserId;

            base.Update(id, entityIn);
        }

        private void AssertUserSpecified()
        {
            if (string.IsNullOrWhiteSpace(UserId))
                throw UserNotSpecifiedException;
        }
    }
}
