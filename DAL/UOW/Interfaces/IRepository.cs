using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.UOW.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<TEntity> GetAsync(TKey id);
        IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);
        Task<EntityEntry<TEntity>> Create(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> DeleteAsync(TKey id);
    }
}