using Microsoft.AspNetCore.Mvc;
using Municipal_Services_Portal.Models;

namespace Municipal_Services_Portal.Controllers
{
    public class ReportIssueController : Controller
    {

        private static IssueLinkedList issues = new IssueLinkedList();

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
    }
}