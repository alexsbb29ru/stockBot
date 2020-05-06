using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Init.Interfaces.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.UOW.Impl
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity: class
    {
        private readonly DbContext _db;
        public Repository(DbContext db)
        {
            _db = db;
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _db.Set<TEntity>().ToList();
        }

        public int GetCount()
        {
            return _db.Set<TEntity>().Count();
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _db.Set<TEntity>().Where(predicate);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var ent = await _db.Set<TEntity>().AddAsync(entity);
            return ent.Entity;
        }

        public void Update(TEntity entity)
        {
            _db.Set<TEntity>().Update(entity);
        }

        public async Task<TEntity> DeleteAsync(TKey id)
        {
            var entity = await _db.Set<TEntity>().FindAsync(id);
            if (entity != null)
                _db.Set<TEntity>().Remove(entity);

            return entity;
        }
    }
}