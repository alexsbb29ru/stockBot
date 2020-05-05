using System;
using System.Threading.Tasks;
using DAL.EF;
using Init.Interfaces.DAL;

namespace DAL.UOW.Impl
{
    public class UnitOfWork<TEntity, TKey>: IUnitOfWork<TEntity, TKey> where TEntity : class
    {
        private readonly BotContext _db;
        private Repository<TEntity, TKey> _botRepository;
        private bool _disposed = false;
        
        public UnitOfWork()
        {
            _db = new BotContext();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _db.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<TEntity, TKey> Repository
            => _botRepository ??= new Repository<TEntity, TKey>(_db);
        
        public async Task<int> Save()
        {
            return await _db.SaveChangesAsync();
        }
    }
}