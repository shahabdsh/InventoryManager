using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InventoryManager.Api.Models;

namespace InventoryManager.Api.Services
{
    public interface IRepositoryService<T> where T : EntityBase
    {
        List<T> Get();
        List<T> Get(Expression<Func<T, bool>> filter);
        /// <summary>
        /// <para>Example query: "type:book;quantity-gt:5;author-ctn:name"</para>
        /// <para>Operations: eq, lt, lte, gt, gte, ctn</para>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        List<T> Get(string query);
        T GetOne(string id);
        T Create(T entity);
        void Update(string id, T entityIn);
        void Remove(T entityIn);
        void Remove(string id);
    }
}
