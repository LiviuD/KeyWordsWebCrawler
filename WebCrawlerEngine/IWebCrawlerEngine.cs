using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawlerEngine
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebCrawlerEngine
    {
        /// <summary>
        /// Crawls the specified URL, for the searched keys.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The string with the postions found</returns>
        Task<string> CrawlAsync(string url, string keys);

        int MaximuNumberOfResults
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the base search URL fro the web crawler.
        /// </summary>
        /// <value>
        /// The base search URL.
        /// </value>
        string BaseSearchURL
        {
            get;
        }

        /// <summary>
        /// Gets or sets the regular expression predicate used to filter the results obtained.
        /// </summary>
        /// <returns>
        /// The regular expression.
        /// </returns>
        Func<Match, string, bool> FilterPredicate
        { get; }

        /// <summary>
        /// Gets or sets the regular expression used to match the results.
        /// </summary>
        /// <value>
        /// The regular expression.
        /// </value>
        string RegularExpression
        {
            get;
        }
    }   
}