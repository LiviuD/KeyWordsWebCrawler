using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using WebCrawlerEngine;

namespace BingWebCrawlerEngine
{
    [Export(nameof(BingWebCrawlerEngine), typeof(IWebCrawlerEngine))]
    public class BingWebCrawlerEngine : WebCrawlerEngine.WebCrawlerEngine, IWebCrawlerEngine
    {
        #region Properties
        private int? maximuNumberOfResults;
        public override int MaximuNumberOfResults
        {
            get
            {
                try
                {
                    return maximuNumberOfResults.HasValue ? maximuNumberOfResults.Value : Int32.Parse(ConfigurationManager.AppSettings.Get("BingWebCrawlerMaximuNUmberOfResults"));
                }
                catch
                {
                    return 100;
                }
            }
            set { maximuNumberOfResults = value; }
        }

        private string baseSearchURL;
        public override string BaseSearchURL
        {
            get
            {
                if (String.IsNullOrEmpty(baseSearchURL))
                    return "https://www.bing.com/search?q={0}&first={1}&FORM=PERE";
                else
                    return baseSearchURL;

            }
            set
            {
                baseSearchURL = value;
            }
        }

        public override Func<Match, string, bool> FilterPredicate
        {
            get
            {
                return (match, target) =>
                {
                    return match.Groups[1].Value.Replace("<strong>", "").Replace("</strong>", "").Contains(target);
                };
            }
        }

        private string regularExpression;
        /// <summary>
        /// Gets or sets the regular expression used to math the results.
        /// </summary>
        /// <value>
        /// The regular expression.
        /// </value>
        public override string RegularExpression
        {
            get { return regularExpression ?? (ConfigurationManager.AppSettings.Get("BingWebCrawlerEngineRegex") ?? "<cite\b[^>] *> (.*?) </cite>"); }
            set { regularExpression = value; }
        }
        #endregion Properties

        /// <summary>
        /// Gets or sets the get HTML.
        /// </summary>
        /// <value>
        /// The get HTML.
        /// </value>
        protected virtual async Task<string> GetHtml(string keys, int page)
        {
            //for bing we get 10 results per pages, so we'll have to get as many pages as neede to reach the maximum number of results
            string search = string.Format(BaseSearchURL, HttpUtility.UrlEncode(keys), page*10);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search);
            using (var webResponse = await request.GetResponseAsync())
            {
                return await ReadFromStreamAsync(webResponse.GetResponseStream(), Encoding.ASCII);
            }
        }

        public async override Task<string> CrawlAsync(string url, string keys)
        {
            var positions = new List<int>();
            StringBuilder sb = new StringBuilder();

            var noOfPages = MaximuNumberOfResults / 10 + 1;
            if (MaximuNumberOfResults % 10 == 0)
                noOfPages = MaximuNumberOfResults / 10;

            for (int i = 0; i < noOfPages; i++)
            {
                sb.Append(await GetHtml(keys, i));
            }
            positions = FindPositions(sb.ToString(), url).ToList();
            if (positions.Count == 0) positions.Add(0);

            return positions
                .Select(x => x.ToString())
                .Aggregate((x, y) => $"{x}, {y}");
        }
    }
}
