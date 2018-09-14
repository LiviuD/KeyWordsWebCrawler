using KeyWordsWebCrawler.Models;
using KeyWordsWebCrawlerDomain;
using KeyWordsWebCrawlerServices;
using Microsoft.AspNet.Identity;
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
        private ISearchResultsHistoryService _service;
        public HomeController(ISearchResultsHistoryService service)
        {
            this._service = service;
        }

        public ActionResult Index()
        {
            ViewBag.DefaultURLValue = ConfigurationManager.AppSettings.Get("DefaultURLValue");
            ViewBag.DefaultKeyValue = ConfigurationManager.AppSettings.Get("DefaultKeyValue");
            ViewData["bodyClass"] = "home";
            var model = new WebCrawalerModel();
            model.SearchResultsHistoryResults = _service.GetAllSearchResultsHistoriesByUserId(User.Identity.GetUserId());
            return View(model);
        }
        
        [HttpPost]
        [OutputCache(Duration = 60)]
        public async System.Threading.Tasks.Task<ActionResult> GetCrawlerResults(string url, string key)
        {
            if(string.IsNullOrEmpty(url))
            {
                url = ConfigurationManager.AppSettings.Get("DefaultURLValue");
            }

            if (string.IsNullOrEmpty(key))
            {
                key = ConfigurationManager.AppSettings.Get("DefaultKeyValue");
            }

            try
            {
                var engineConfig = ConfigurationManager.AppSettings.Get("WebCrawlerEngine");
                var engine = MefConfig.Container.GetExport<IWebCrawlerEngine>(engineConfig).Value;
                var results = await engine.CrawlAsync(url, key);
                var searchResultsHistory = this._service.SaveSearchResultsHistory(new SearchResultsHistory() {UserId = User.Identity.GetUserId(), Key = key, Url = url, Results = results });
                return Json(new { SearchResultsHistory = searchResultsHistory, FormatedDate = searchResultsHistory.CreatedDate.ToString("dd.MM.yyyy hh:mm") }, JsonRequestBehavior.AllowGet);
            }
            catch(ImportCardinalityMismatchException)
            {
                return Json("An error occured. Please check that the system is properlly configured.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("An error occured. Please contact your administrator.", JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            //TODO
            ViewBag.Message = "This is a plug-able web-crawler application. ";
            ViewData["bodyClass"] = "about";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            //TODO
            ViewData["bodyClass"] = "contact";
            ViewBag.Message = "Please do contact me if you have any question";

            return View();
        }
    }
}