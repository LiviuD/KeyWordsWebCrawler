using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyWordsWebCrawlerDomain
{
    public class SearchResultsHistory : IEntity
    {
        public int Id
        {
            get;
            set;
        }

        [Required]
        public string UserId
        {
            get;
            set;
        }

        [Required]
        public DateTime CreatedDate
        {
            get;
            set;
        }

        [Required]
        public string Url
        {
            get;
            set;
        }

        [Required]
        public string Key
        {
            get;
            set;
        }

        public string Results
        {
            get;
            set;
        }

    }
}
