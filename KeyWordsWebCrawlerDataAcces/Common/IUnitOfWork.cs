using KeyWordsWebCrawlerDomain;
using System;

namespace KeyWordsWebCrawlerDataAcces.Common
{
    public interface IUnitOfWork :  IDisposable
    {
        int SaveChanges();

        IUnitOfWork GetNewContext(bool disposeExisting = false);
        IBaseRepository<T> Repository<T>() where T : class, IEntity;
    }
}
