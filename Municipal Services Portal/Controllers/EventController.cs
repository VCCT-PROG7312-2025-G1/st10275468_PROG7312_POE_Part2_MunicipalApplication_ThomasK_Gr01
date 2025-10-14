using Microsoft.AspNetCore.Mvc;
using Municipal_Services_Portal.Models;
using System.Linq;

namespace Municipal_Services_Portal.Controllers
{
    public class EventController : Controller
    {
        // Sorted Dictionary used to store events by date
        private static SortedDictionary<DateTime, List<Event>> eventsSortedDate = new SortedDictionary<DateTime, List<Event>>();

        // HashSet used to store unique categories
        private static HashSet<string> categories = new HashSet<string>();

        // Stack used for recently viewed events
        private static Stack<Event> recentlyViewedEvents = new Stack<Event>();

        //List to store and track the user searches in the events page 
        private static List<string> userSearches = new List<string>();

        static EventController()
        {
            // Sample events
            AddEvent("Event 1", "Category 1", new DateTime(2025, 10, 5), "Event 1 description", "Event 1 location", "/EventImages/marathon.jpg");
            AddEvent("Event 2", "Category 2", new DateTime(2025, 10, 12), "Event 2 description", "Event 2 location", "");
            AddEvent("Event 3", "Category 3", new DateTime(2025, 10, 18), "Event 3 description", "Event 3 location", "");
            AddEvent("Event 4", "Category 1", new DateTime(2025, 10, 22), "Event 4 description", "Event 4 location", "");
            AddEvent("Event 5", "Category 2", new DateTime(2025, 10, 28), "Event 5 description", "Event 5 location", "");
            AddEvent("Event 6", "Category 3", new DateTime(2025, 11, 1), "Event 6 description", "Event 6 location", "");
            AddEvent("Event 7", "Category 1", new DateTime(2025, 11, 5), "Event 7 description", "Event 7 location", "");
            AddEvent("Event 8", "Category 2", new DateTime(2025, 11, 9), "Event 8 description", "Event 8 location", "");
            AddEvent("Event 9", "Category 3", new DateTime(2025, 11, 13), "Event 9 description", "Event 9 location", "");
            AddEvent("Event 10", "Category 1", new DateTime(2025, 11, 17), "Event 10 description", "Event 10 location", "");
            AddEvent("Event 11", "Category 2", new DateTime(2025, 11, 21), "Event 11 description", "Event 11 location", "");
            AddEvent("Event 12", "Category 3", new DateTime(2025, 11, 25), "Event 12 description", "Event 12 location", "");
            AddEvent("Event 13", "Category 1", new DateTime(2025, 11, 29), "Event 13 description", "Event 13 location", "");
            AddEvent("Event 14", "Category 2", new DateTime(2025, 12, 3), "Event 14 description", "Event 14 location", "");
            AddEvent("Event 15", "Category 3", new DateTime(2025, 12, 7), "Event 15 description", "Event 15 location", "");


        }

        private static void AddEvent(string name, string category, DateTime date, string description, string location, string imgPath)
        {
            Event newEvent = new Event(name, category, description, location, date, imgPath);

            if (!eventsSortedDate.ContainsKey(date))
                eventsSortedDate[date] = new List<Event>();

            eventsSortedDate[date].Add(newEvent);
            categories.Add(category);
        }


        [HttpGet]
        public IActionResult Events(string search = "", string categoryFilter = "", string sortOrder = "")
        {
            var eventsList = eventsSortedDate.SelectMany(e => e.Value).ToList();

            if (!string.IsNullOrEmpty(categoryFilter))
                eventsList = eventsList.Where(e => e.Category == categoryFilter).ToList();

            if (!string.IsNullOrEmpty(search))
                eventsList = eventsList.Where(e => e.eventName.ToLower().Contains(search.ToLower())).ToList();



            eventsList = sortOrder switch
            {
                "dateAsc" => eventsList.OrderBy(e => e.Date).ToList(),
                "dateDesc" => eventsList.OrderByDescending(e => e.Date).ToList(),
                "nameAsc" => eventsList.OrderBy(e => e.eventName).ToList(),
                "nameDesc" => eventsList.OrderByDescending(e => e.eventName).ToList(),
                "categoryAsc" => eventsList.OrderBy(e => e.Category).ToList(),
                "categoryDesc" => eventsList.OrderByDescending(e => e.Category).ToList(),
                _ => eventsList.OrderBy(e => e.Date).ToList()
            };

            if (!userSearches.Contains(search.ToLower()))
            {
                userSearches.Insert(0, search.ToLower());
                if (userSearches.Count > 5)
                {
                    userSearches.RemoveAt(userSearches.Count - 1);
                }

            }
            List<Event> suggestedEvents = new List<Event>();

            if (!string.IsNullOrWhiteSpace(search)) { 

                suggestedEvents = eventsSortedDate.SelectMany(e => e.Value).Where(ev => userSearches.Any(term => ev.eventName.ToLower().Contains(term))).Except(eventsList).Take(5).ToList();
            }

            var announcements = new List<Announcement>
            {
                new Announcement { name = "Road works", description = "Road maintenance on 33 Boundry road", Date = DateTime.Now.AddDays(8) },
                new Announcement { name = "Apartments", description = "New apartment block being built in Observatory", Date = DateTime.Now.AddDays(54) },
                new Announcement { name = "New Electricity regulations", description = "Electricity regulations are active from Friday", Date = DateTime.Now.AddDays(3) },

            };


            ViewBag.Categories = categories.OrderBy(c => c).ToList();
            ViewBag.RecentlyViewed = recentlyViewedEvents.ToList();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = categoryFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SuggestedEvents = suggestedEvents;
            ViewBag.Announcements = announcements;

            return View(eventsList);
        }

        [HttpPost]
        public IActionResult ViewEvent(string eventName)
        {
            Event evt = eventsSortedDate.SelectMany(e => e.Value)
                    .FirstOrDefault(ev => ev.eventName == eventName);

            if (evt != null)
            {
                recentlyViewedEvents = new Stack<Event>(recentlyViewedEvents.Where(e => e.eventName != eventName));
                recentlyViewedEvents.Push(evt);

                if (recentlyViewedEvents.Count > 5)
                    recentlyViewedEvents = new Stack<Event>(recentlyViewedEvents.Take(5).Reverse());
            }
            return RedirectToAction("Events");
        }
    }
}
