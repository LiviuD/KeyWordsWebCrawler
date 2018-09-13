using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KeyWordsWebCrawler.Models
{
    public class WebCrawalerModel
    {
        [RegularExpression(@"^((ht|f)tp(s?)\:\/\/){0,1}[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)\.([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$", ErrorMessage ="The text enter might not be an URL, please check")]
        public string Url
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;

        }

    }
}