using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleWebCrawlerEngine.Tests
{
    public class GoogleWebCrawlerEngineMock : GoogleWebCrawlerEngine
    {
        protected override async Task<string> GetHtml(string keys)
        {
            using (FileStream fileStream = File.Open("test.html", FileMode.OpenOrCreate))
            {
                return await ReadFromStreamAsync(fileStream, Encoding.ASCII);
            }
        }
    }
}
