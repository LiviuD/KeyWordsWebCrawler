using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoogleWebCrawlerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleWebCrawlerEngine.Tests
{
    /// <summary>
    /// These tests rely on the Test.html file that acts as a mock for the web stream 
    /// </summary>
    [TestClass()]
    public class GoogleWebCrawlerEngineTests
    {
        [TestMethod()]
        public void WhenSearchingThatMatchesTheContentOfTheTestFile()
        {
            //Arrange
            var googleWebCrawlerEngine = new GoogleWebCrawlerEngineMock();
            //Act
            var result = googleWebCrawlerEngine.CrawlAsync("www.hotnews.ro", "hotnews").Result;
            //Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result) && result == "1, 9" );
        }

        [TestMethod()]
        public void WhenSearchingThatDoesNotMatchesTheContentOfTheTestFile()
        {
            //Arrange
            var googleWebCrawlerEngine = new GoogleWebCrawlerEngineMock();
            //Act
            var result = googleWebCrawlerEngine.CrawlAsync("www.sympli.com.au", "online title search").Result;
            //Assert
            Assert.IsTrue(!String.IsNullOrEmpty(result) && result == "0");
        }
    }
}