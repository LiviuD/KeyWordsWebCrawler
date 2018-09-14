using KeyWordsWebCrawlerDataAcces;
using KeyWordsWebCrawlerDataAcces.Common;
using KeyWordsWebCrawlerDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyWordsWebCrawlerServices
{
    public class SearchResultsHistoryService : BaseService, ISearchResultsHistoryService
    {
        public SearchResultsHistoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the search results history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public SearchResultsHistory GetSearchResultsHistory(int id)
        {
            return UnitOfWork.Repository<SearchResultsHistory>().Find(x => x.Id == id);
        }

        /// <summary>
        /// Gets all search results histories by user identifier.
        /// </summary>  
        /// <returns></returns>
        public IEnumerable<SearchResultsHistory> GetAllSearchResultsHistoriesByUserId(string userId)
        {
            return UnitOfWork.Repository<SearchResultsHistory>()
                .Filter(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(30)
                .AsEnumerable();
        }

        /// <summary>
        /// Gets all search results histories.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SearchResultsHistory> GetAllSearchResultsHistories()
        {
            return UnitOfWork.Repository<SearchResultsHistory>().All();
        }

        /// <summary>
        /// Saves the search results history.
        /// </summary>
        /// <param name="restaurant">The restaurant.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">The restaurant entity is not valid! - null</exception>
        public SearchResultsHistory SaveSearchResultsHistory(SearchResultsHistory searchResultsHistory)
        {
            searchResultsHistory.CreatedDate = DateTime.Now;
            UnitOfWork.Repository<SearchResultsHistory>().Create(searchResultsHistory);
            UnitOfWork.SaveChanges();
            return searchResultsHistory;
        }
    }
}
