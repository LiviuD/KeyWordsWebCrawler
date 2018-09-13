using Microsoft.VisualStudio.TestTools.UnitTesting;
using BingWebCrawlerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWebCrawlerEngine.Tests
{
    [TestClass()]
    public class BingWebCrawlerEngineTests
    {
        [TestMethod()]
        public void WhenSearchingThatMatchesTheContentOfTheTestFile()
        {
            //Arrange
            var webCrawlerEngine = new BingWebCrawlerEngineMock();
            //Act
            var result = webCrawlerEngine.CrawlAsync("www.hotnews.ro", "hotnews").Result;
            //Assert
            Assert.IsTrue(result == "1");
        }

        [TestMethod()]
        public void WhenSearchingThatDoesNotMatchesTheContentOfTheTestFile()
        {
            //Arrange
            var webCrawlerEngine = new BingWebCrawlerEngineMock();
            //Act
            var result = webCrawlerEngine.CrawlAsync("www.sympli.com.au", "online title search").Result;
            //Assert
            Assert.IsTrue(result == "0");
        }
    }
}