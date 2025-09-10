using Microsoft.AspNetCore.Mvc;
using Municipal_Services_Portal.Models;

namespace Municipal_Services_Portal.Controllers
{
    public class ReportIssueController : Controller
    {

        public static IssueLinkedList issues { get; } = new IssueLinkedList();

        static ReportIssueController()
        {
            //Sample data that will be on the website when opened
            var issue1 = new Issue("2 Howe Road, Observatory", "Road", "Road is flooded", "");
            var issue2 = new Issue("41 Main Road, Bergvliet", "Electrical", "Street light down", "");

            issue1.Status = "Resolved";
            issue2.Status = "In progress";

            issues.AddIssue(issue1);
            issues.AddIssue(issue2);
               
        }

        [HttpGet]
        public IActionResult ReportIssues()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReportIssues(string location, string category, string description, Microsoft.AspNetCore.Http.IFormFile media)
        {
            string mediaPath = "";
            if (media != null && media.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/IssueImages");

                if (!Directory.Exists(folder)) { 
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, media.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    media.CopyTo(stream);
                }

                mediaPath = "/IssueImages/" + media.FileName;

            }

            Issue newIssue = new Issue(location, category, description, mediaPath);
            issues.AddIssue(newIssue);
            ViewBag.Message = "Issue submitted";

            return View();
        }

        [HttpGet]
        public IActionResult ViewIssues()
        {
            Issue[] issueArray = issues.ToArray();
            return View(issueArray);
        }

    }
}