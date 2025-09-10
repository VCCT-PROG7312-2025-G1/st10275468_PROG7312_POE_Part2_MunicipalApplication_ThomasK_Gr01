using Microsoft.AspNetCore.Mvc;
using Municipal_Services_Portal.Models;
using System.Diagnostics;

namespace Municipal_Services_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        public IActionResult Index()
        {
            Issue[] allIssues = ReportIssueController.issues.ToArray();
            
            int totalIssues = allIssues.Length;
            int pendingIssues = allIssues.Count(i => i.Status == "Pending");

            string mostRecentIssue = allIssues.Length > 0
               ? allIssues.OrderByDescending(i => i.DateSubmitted).First().Location
               : "No issues";

            string mostRecentIssueDes = allIssues.Length > 0
                ? allIssues.OrderByDescending(i => i.DateSubmitted).First().Description
                : "No issues";


            ViewData["TotalIssues"] = totalIssues;
            ViewData["PendingIssues"] = pendingIssues;
            ViewData["MostRecentIssueLocation"] = mostRecentIssue;
            ViewData["MostRecentIssueDescription"] = mostRecentIssueDes;

            return View();
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
