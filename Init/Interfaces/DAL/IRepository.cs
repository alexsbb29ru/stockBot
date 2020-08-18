using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Init.Interfaces.DAL
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        int GetCount();
        Task<TEntity> GetAsync(TKey id);
        IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        TEntity Update(TEntity entity);
        Task<TEntity> DeleteAsync(TKey id);
    }
}