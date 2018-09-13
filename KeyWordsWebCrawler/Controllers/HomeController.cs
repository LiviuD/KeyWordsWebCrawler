using KeyWordsWebCrawler.Models;
using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Web.Mvc;
using WebCrawlerEngine;

namespace KeyWordsWebCrawler.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new WebCrawalerModel();
            return View(model);
        }
        
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetCrawlerResults(string url, string key)
        {
            if(string.IsNullOrEmpty(url))
            {
                url = "www.sympli.com.au";
            }

            if (string.IsNullOrEmpty(key))
            {
                key = "online title search";
            }
            try
            {
                var engineConfig = ConfigurationManager.AppSettings.Get("WebCrawlerEngine");
                var engine = MefConfig.Container.GetExport<IWebCrawlerEngine>(engineConfig).Value;
                var results = await engine.CrawlAsync(url, key);
                return Json($"Results are: {results}", JsonRequestBehavior.AllowGet);
            }
            catch(ImportCardinalityMismatchException)
            {
                return Json("An error occured. Please check that the system is properlly configured.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("An error occured. Please contact your adminsitrator.", JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            //TODO
            ViewBag.Message = "This is a plug-able web-crawler application. ";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            //TODO
            ViewBag.Message = "Please do contact me if you have any question";

            return View();
        }
    }
}