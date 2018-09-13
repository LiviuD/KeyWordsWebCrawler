using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebCrawlerEngine
{
    /// <summary>
    /// Base class for the web crawlers
    /// </summary>
    /// <seealso cref="WebCrawlerEngine.IWebCrawlerEngine" />
    public abstract class WebCrawlerEngine : IWebCrawlerEngine
    {
        #region Properties
        /// <summary>
        /// Gets or sets the maximu n umber of results.
        /// </summary>
        /// <value>
        /// The maximu n umber of results.
        /// </value>
        public abstract int MaximuNumberOfResults
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
        public abstract string BaseSearchURL
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the regular expression predicate used to filter the results obtained.
        /// </summary>
        /// <returns>
        /// The regular expression.
        /// </returns>
        public abstract Func<Match, string, bool> FilterPredicate
        {
            get;
        }

        /// <summary>
        /// Gets or sets the regular expression used to match the results.
        /// </summary>
        /// <value>
        /// The regular expression.
        /// </value>
        public abstract string RegularExpression
        {
            get;
            set;
        }
        #endregion Properties

        #region Helper methods
        /// <summary>
        /// Finds the position.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="targetedString">The URL.</param>
        /// <param name="lookup">The lookup string.</param>
        /// <returns></returns>
        protected virtual IEnumerable<int> FindPositions(string html, string targetedString)
        {
            MatchCollection matches = Regex.Matches(html, RegularExpression);
            if (matches.Count == 0)
                yield return 0;

            for (int i = 0; i < Math.Min(matches.Count, MaximuNumberOfResults); i++)
            {
                if (FilterPredicate(matches[i], targetedString))
                    yield return i + 1;
            }
        }

        /// <summary>
        /// Reads from stream asynchronous.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        protected async Task<string> ReadFromStreamAsync(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                return await reader.ReadToEndAsync();
            }
        }
        #endregion Helper methods

        /// <summary>
        /// Crawls the specified URL, for the searched keys.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The string with the postions found</returns>
        public abstract Task<string> CrawlAsync(string url, string keys);
    }
}
