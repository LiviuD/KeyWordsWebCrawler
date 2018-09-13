using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWebCrawlerEngine.Tests
{
    public class BingWebCrawlerEngineMock : BingWebCrawlerEngine
    {
        protected override async Task<string> GetHtml(string keys, int page)
        {
            using (FileStream fileStream = File.Open("test.html", FileMode.OpenOrCreate))
            {
                return await ReadFromStreamAsync(fileStream, Encoding.ASCII);
            }
        }
    }
}
