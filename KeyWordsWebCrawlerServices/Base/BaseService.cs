using KeyWordsWebCrawlerDataAcces.Common;

namespace KeyWordsWebCrawlerServices
{
    public class BaseService : IService
    {
        protected IUnitOfWork UnitOfWork;

        #region Constructor

        public BaseService()
        { }

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion

    }
}
