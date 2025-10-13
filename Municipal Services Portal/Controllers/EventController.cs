using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Municipal_Services_Portal.Models;
namespace Municipal_Services_Portal.Controllers
{
    public class EventController : Controller
    {
        //Sorted Dictionary used to store the events sorted by date
        private static SortedDictionary<DateTime, List<Event>> eventsSortedDate = new SortedDictionary<DateTime, List<Event>>();

        //Hashset used to store the categories of the events
        private static HashSet<string> categories = new HashSet<string>();

        //Stack used to store the recently viewed events
        private static Stack<Event> recentlyViewedEvents = new Stack<Event>();

        private static Dictionary<string, int> searchTracker = new Dictionary<string, int>();

        static EventController()
        {
            //Sample events
            AddEvent("Event 1", "Category 1", new DateTime(2025, 10, 5), "Event 1 description", "Event 1 location");
            AddEvent("Event 2", "Category 2", new DateTime(2025, 10, 12), "Event 2 description", "Event 2 location");
            AddEvent("Event 3", "Category 3", new DateTime(2025, 10, 18), "Event 3 description", "Event 3 location");
            AddEvent("Event 4", "Category 1", new DateTime(2025, 10, 22), "Event 4 description", "Event 4 location");
            AddEvent("Event 5", "Category 2", new DateTime(2025, 10, 28), "Event 5 description", "Event 5 location");
            AddEvent("Event 6", "Category 3", new DateTime(2025, 11, 1), "Event 6 description", "Event 6 location");
            AddEvent("Event 7", "Category 1", new DateTime(2025, 11, 5), "Event 7 description", "Event 7 location");
            AddEvent("Event 8", "Category 2", new DateTime(2025, 11, 9), "Event 8 description", "Event 8 location");
            AddEvent("Event 9", "Category 3", new DateTime(2025, 11, 13), "Event 9 description", "Event 9 location");
            AddEvent("Event 10", "Category 1", new DateTime(2025, 11, 17), "Event 10 description", "Event 10 location");
            AddEvent("Event 11", "Category 2", new DateTime(2025, 11, 21), "Event 11 description", "Event 11 location");
            AddEvent("Event 12", "Category 3", new DateTime(2025, 11, 25), "Event 12 description", "Event 12 location");
            AddEvent("Event 13", "Category 1", new DateTime(2025, 11, 29), "Event 13 description", "Event 13 location");
            AddEvent("Event 14", "Category 2", new DateTime(2025, 12, 3), "Event 14 description", "Event 14 location");
            AddEvent("Event 15", "Category 3", new DateTime(2025, 12, 7), "Event 15 description", "Event 15 location");

        }
        /// <summary>
        /// Method that adds an event to the sorted dictionary and the event categories to the category hashset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="date"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        private static void AddEvent(string name, string category, DateTime date, string description, string location)
        {
           
            Event newEvent = new Event(name, category, description, location, date);

            if (!eventsSortedDate.ContainsKey(date))
            {
                eventsSortedDate[date] = new List<Event>();
            }
            eventsSortedDate[date].Add(newEvent);

            categories.Add(category);
        
            
        }


        [HttpGet]
        public IActionResult Events()
        {
            var eventsList = eventsSortedDate.SelectMany(e => e.Value).OrderBy(e => e.Date).ToList();
            ViewBag.Categories = categories.OrderBy(c => c).ToList();
            ViewBag.RecentlyViewed = recentlyViewedEvents.ToList();
         
            return View(eventsList);
        }
    }
}
