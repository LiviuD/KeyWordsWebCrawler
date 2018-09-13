using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using WebCrawlerEngine;

namespace GoogleWebCrawlerEngine
{
    [Export(nameof(GoogleWebCrawlerEngine), typeof(IWebCrawlerEngine))]
    public class GoogleWebCrawlerEngine : WebCrawlerEngine.WebCrawlerEngine, IWebCrawlerEngine
    {
        #region Properties
        private int? maximuNUmberOfResults;
        public override int MaximuNumberOfResults
        {
            get
            {
                try
                {
                    return maximuNUmberOfResults.HasValue ? maximuNUmberOfResults.Value : Int32.Parse(ConfigurationManager.AppSettings.Get("GoogleWebCrawlerMaximuNUmberOfResults"));
                }
                catch
                {
                    return 100;
                }
            }
            set { maximuNUmberOfResults = value; }
        }

        private string baseSearchURL;
        public override string BaseSearchURL
        {
            get
            {
                if (String.IsNullOrEmpty(baseSearchURL))
                    return "https://www.google.com.au/search?num={1}&q={0}&btnG=Search";
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
                    return match.Groups[1].Value.Replace("<b>", "").Replace("</b>", "").Contains(target);
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
            get { return regularExpression ?? (ConfigurationManager.AppSettings.Get("GoogleWebCrawlerEngineRegex") ?? "<cite\b[^>] *> (.*?) </cite>"); }
            set { regularExpression = value; }
        }
        #endregion Properties

        /// <summary>
        /// Gets or sets the get HTML.
        /// </summary>
        /// <value>
        /// The get HTML.
        /// </value>
        protected virtual async Task<string> GetHtml(string keys)
        {
            string search = string.Format(BaseSearchURL, HttpUtility.UrlEncode(keys), MaximuNumberOfResults);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search);
            using (var webResponse = await request.GetResponseAsync())
            {
                return await ReadFromStreamAsync(webResponse.GetResponseStream(), Encoding.ASCII);
            }
        }

        public async override Task<string> CrawlAsync(string url, string keys)
        {
            var positions = new List<int>();
            var html = await GetHtml(keys);
            positions = FindPositions(html, url).ToList();
            if (positions.Count == 0) positions.Add(0);

            return positions
                .Select(x => x.ToString())
                .Aggregate((x, y) => $"{x}, {y}");
        }
    }
}
