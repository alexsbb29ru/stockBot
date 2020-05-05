using System;
using System.Threading.Tasks;

namespace Init.Interfaces.DAL
{
    public interface IUnitOfWork<TEntity, TKey> : IDisposable where TEntity : class
    {
        IRepository<TEntity, TKey> Repository { get; }
        Task<int> Save();
    }
}