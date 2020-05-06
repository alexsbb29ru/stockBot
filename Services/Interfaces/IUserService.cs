using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService<TEntity, TKey>
    {
        IEnumerable<TEntity> GetAll();
        int GetCount();
        Task<TEntity> GetAsync(TKey id);
        IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> DeleteAsync(TKey id);
    }
}