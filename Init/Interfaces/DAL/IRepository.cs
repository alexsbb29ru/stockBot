﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Init.Interfaces.DAL
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<TEntity> GetAsync(TKey id);
        IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> DeleteAsync(TKey id);
    }
}