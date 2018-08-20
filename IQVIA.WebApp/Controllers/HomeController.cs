using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IQVIA.Common;
using IQVIA.WebApp.Extensions;
using IQVIA.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IQVIA.WebApp.Controllers
{
    public class HomeController : Controller
    {
        protected IConfiguration _configuration;
        protected TweetLoader _loader;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _loader = new TweetLoader(new TweetService(_configuration.GetValue("AppSettings:TweetServiceUrl", "")));
        }

        public IActionResult Index()
        {
            return View(new SearchViewModel());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search(SearchViewModel model)
        {
            // test project, so no datetime binders. we assume that incoming dates are treated as UTC dates
            model.StartDate = DateTime.SpecifyKind(model.StartDate, DateTimeKind.Utc);
            model.EndDate = DateTime.SpecifyKind(model.EndDate, DateTimeKind.Utc);
            var response = await _loader.LoadTweetsByDate(model.StartDate, model.EndDate);
            model.Tweets = response.Tweets;
            model.ErrorMessage = response.ErrorMessage;

            if (Request.IsAjaxRequest())
            {
                return View("_SearchPartial", model);
            }
            else
                return View(model);
        }

    }
}