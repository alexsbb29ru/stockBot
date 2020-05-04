using System;
using System.Threading.Tasks;

namespace DAL.UOW.Interfaces
{
    public interface IUnitOfWork<TEntity, TKey> : IDisposable where TEntity : class
    {
        IRepository<TEntity, TKey> Repository { get; }
        Task<int> Save();
    }
}