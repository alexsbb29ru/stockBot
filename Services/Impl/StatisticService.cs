using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Init.Interfaces.DAL;
using Services.Interfaces;

namespace Services.Impl
{
    public class StatisticService<TEntity, TKey> : IStatisticService<TEntity, TKey> where TEntity : class
    {
        private readonly IUnitOfWork<TEntity, TKey> _unitOfWork;

        public StatisticService(IUnitOfWork<TEntity, TKey> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IEnumerable<TEntity> GetAll()
        {
            return _unitOfWork.Repository.GetAll();
        }

        public int GetCount()
        {
            return _unitOfWork.Repository.GetCount();
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await _unitOfWork.Repository.GetAsync(id).ConfigureAwait(false);
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _unitOfWork.Repository.Find(predicate);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var stat = await _unitOfWork.Repository.CreateAsync(entity).ConfigureAwait(false);
            await _unitOfWork.Save().ConfigureAwait(false);
            return stat;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var updatedEnt = _unitOfWork.Repository.Update(entity);
            await _unitOfWork.Save().ConfigureAwait(false);
            return updatedEnt;
        }

        public async Task<TEntity> DeleteAsync(TKey id)
        {
            var entity = await _unitOfWork.Repository.DeleteAsync(id).ConfigureAwait(false);
            return entity;
        }
    }
}