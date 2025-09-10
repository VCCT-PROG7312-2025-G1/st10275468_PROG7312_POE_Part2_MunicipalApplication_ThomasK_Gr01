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

            //Retrieving the total and pending issues
            int totalIssues = allIssues.Length;
            int pendingIssues = allIssues.Count(i => i.Status == "Pending");

            //Retrieving the most recent issue location
            string mostRecentIssue = allIssues.Length > 0
               ? allIssues.OrderByDescending(i => i.DateSubmitted).First().Location
               : "No issues";

            //Retrieving the most recent issue description
            string mostRecentIssueDes = allIssues.Length > 0
                ? allIssues.OrderByDescending(i => i.DateSubmitted).First().Description
                : "No issues";

            //Retrieving details such as total number of issues etc, to display on the info card
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
