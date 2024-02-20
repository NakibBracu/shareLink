using Microsoft.AspNetCore.Mvc;
using Share.UI.Models;
using System.Diagnostics;

namespace Share.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory  = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShareViewOnFacebook()
        {
            // Get the URL of the action to share
            var actionUrl = Url.Action("Index", "Home", null, Request.Scheme);

            ViewBag.ActionUrl = actionUrl; // Passing the action URL to the view

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShareViewOnFacebook(string message)
        {
            // Get the URL of the action to share
            var actionUrl = Url.Action("Index", "Home", null, Request.Scheme);

            // Prepare the content to post on Facebook
            var content = new StringContent($"message={message}&link={actionUrl}");

            // Create an HttpClient instance
            var httpClient = _httpClientFactory.CreateClient();

            // Post to Facebook's Graph API to share the link
            var response = await httpClient.PostAsync("https://graph.facebook.com/me/feed", content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Handle the failure scenario
                return BadRequest("Failed to share on Facebook.");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
