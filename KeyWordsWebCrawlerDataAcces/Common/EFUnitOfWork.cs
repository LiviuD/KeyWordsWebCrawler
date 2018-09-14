using System;
using System.Collections.Concurrent;
using System.Data.Entity.Validation;
using System.Data.Entity;
using KeyWordsWebCrawlerDomain;

namespace KeyWordsWebCrawlerDataAcces.Common
{
    public class EFUnitOfWork : IUnitOfWork
    {

        protected readonly KeyWordsWebCrawlerData _context;
        public EFUnitOfWork(KeyWordsWebCrawlerData context) 
        {
            _context = context;
        }

        private readonly ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();
        public IBaseRepository<T> Repository<T>() where T : class, IEntity
        {
            var type = typeof(T);
            var typeName = type.Name;
            return (IBaseRepository<T>)_repositories.GetOrAdd(typeName, t =>
            {
                return new EFRepository<T>(_context);
            });

            throw new NotImplementedException();
        }

        public int SaveChanges()
        {

            try
            {
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //log?
                throw ex;
            }
        }

        public IUnitOfWork GetNewContext(bool disposeExisting = false)
        {
            if (disposeExisting)
                this.Dispose();
            KeyWordsWebCrawlerData context = new KeyWordsWebCrawlerData();
            return new EFUnitOfWork(context);
        }

        public static EFUnitOfWork GetNewContext()
        {
            KeyWordsWebCrawlerData context = new KeyWordsWebCrawlerData();
            return new EFUnitOfWork(context);
        }

        public void Dispose()
        {
            try
            {
                _context?.Dispose();
            }
            catch { }
        }

        private DbContextTransaction _transaction = null;
        public IDisposable BeginTransaction()
        {
            if (null != _transaction)
                throw new ApplicationException("A transaction is already opened!");

            _transaction = _context.Database.BeginTransaction();
            return (IDisposable)_transaction;
        }

        public void CommitTransaction()
        {
            if (null == _transaction)
                throw new ApplicationException("Transaction not found!");

            _transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (null != _transaction)
                _transaction.Rollback();
        }
    }
}
