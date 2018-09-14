using KeyWordsWebCrawlerDomain;
using System.Collections.Generic;

namespace KeyWordsWebCrawlerServices
{
    public interface ISearchResultsHistoryService
    {
        /// <summary>
        /// Gets the search results history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        SearchResultsHistory GetSearchResultsHistory(int id);
        /// <summary>
        /// Gets all search results histories.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchResultsHistory> GetAllSearchResultsHistories();
        /// <summary>
        /// Gets all search results histories by user identifier.
        /// </summary>  
        /// <returns></returns>
        IEnumerable<SearchResultsHistory> GetAllSearchResultsHistoriesByUserId(string userId);
        /// <summary>
        /// Saves the search results history.
        /// </summary>
        /// <param name="restaurant">The restaurant.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">The restaurant entity is not valid! - null</exception>
        SearchResultsHistory SaveSearchResultsHistory(SearchResultsHistory restaurant);
    }
}