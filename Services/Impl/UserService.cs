using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Init.Interfaces.DAL;
using Services.Interfaces;

namespace Services.Impl
{
    public class UserService<TEntity, TKey> : IUserService<TEntity, TKey> where TEntity : class
    {
        private readonly IUnitOfWork<TEntity, TKey> _unitOfWork;

        public UserService(IUnitOfWork<TEntity, TKey> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Get all users from database
        /// </summary>
        /// <returns>List of users</returns>
        public IEnumerable<TEntity> GetAll()
        {
            return _unitOfWork.Repository.GetAll();
        }
        /// <summary>
        /// Get user count
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return _unitOfWork.Repository.GetCount();
        }

        /// <summary>
        /// Get user from database by user id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        public async Task<TEntity> GetAsync(TKey id)
        {
            return await _unitOfWork.Repository.GetAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Find user in database
        /// </summary>
        /// <param name="predicate">Predicate to find</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _unitOfWork.Repository.Find(predicate);
        }

        /// <summary>
        /// Create user and save to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var user = await _unitOfWork.Repository.CreateAsync(entity).ConfigureAwait(false);
            await _unitOfWork.Save().ConfigureAwait(false);
            return user;
        }

        /// <summary>
        /// Update user in database
        /// </summary>
        /// <param name="entity">Updated entity</param>
        public async Task<TEntity> Update(TEntity entity)
        {
            var updatedEnt = _unitOfWork.Repository.Update(entity);
            await _unitOfWork.Save().ConfigureAwait(false);
            return updatedEnt;
        }

        /// <summary>
        /// Delete user from database
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        public async Task<TEntity> DeleteAsync(TKey id)
        {
            var entity = await _unitOfWork.Repository.DeleteAsync(id).ConfigureAwait(false);
            return entity;
        }
    }
}