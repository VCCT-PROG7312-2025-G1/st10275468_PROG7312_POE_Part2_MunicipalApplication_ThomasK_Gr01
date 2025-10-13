using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Municipal_Services_Portal.Controllers
{
    public class EventController : Controller
    {


        public IActionResult Events()
        {
            return View();
        }
    }
}
