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
            // Sample sport events
            AddEvent("ABSA 10km race", "Sport", new DateTime(2025, 10, 5), "Take part in Absa's family friendly 10km running race. All the money involved will be donated directly to a charity.", "Milnerton", "/EventImages/Absa10km.jpg");
            AddEvent("ABSA Cape Epic", "Sport", new DateTime(2025, 11, 17), "This is a 700km cycling event that runs over 8 days and takes cyclists through the mountains of Western Cape down to Cape Town.", "Durbinville", "/EventImages/AbsaCapeEpic.jpg");
            AddEvent("Cape Town Cycle Tour", "Sport", new DateTime(2026, 2, 25), "Worlds biggest cycling event with over 30000 cyclists competing in a 110km route running around Cape Town.", "Fishoek", "/EventImages/CycleTour.jpg");
            AddEvent("Discovery Triathlon", "Sport", new DateTime(2025, 12, 10), "Take part in Cape Towns biggest Triathlon event.", "V&A Waterfront", "/EventImages/Triathlon.jpg");
            AddEvent("Two Oceans Marathon", "Sport", new DateTime(2026, 3, 15), "This event consists of 3 different runs. The 5km fun run, 21km half marathon and 56 ultra-marathon that takes you through Cape Towns beautiful scenery.", "Muizenberg", "/EventImages/TwoOceans.jpg");
            AddEvent("Sanlam Marathon", "Sport", new DateTime(2025, 10, 28), "The Sanlam marathon consists of over 20000 athletes from over 50 countries and has achieved Gold label status.", "Milnerton", "/EventImages/Sanlam.jpg");
            AddEvent("Cape Town Sevens", "Sport", new DateTime(2025, 12, 17), "2 Days of non-stop rugby and festivity in the stadium.", "Cape Town Stadium", "/EventImages/Sevens.jpg");
            AddEvent("Cape Town 10s", "Sport", new DateTime(2025, 10, 28), "Cape Towns social sport event featuring rugby, netball, volleyball, football and live entertainment.", "GreenPoint Track", "/EventImages/10s.jpg");

            //Sample Culture and creativity events
            AddEvent("Jazz Festival", "Culture and Creativity", new DateTime(2025, 11, 25), "One of Africa's biggest Jazz events with international and national artists.", "Baxter Theatre", "/EventImages/JazzFesti.jpg");
            AddEvent("Cape Town Carnival", "Culture and Creativity", new DateTime(2026, 3, 5), "Watch Cape Towns streets come to life with music, dancers and entertainment.", "Bree Street", "/EventImages/Carnival.jpg");
            AddEvent("Cape Town Funny Festival", "Culture and Creativity", new DateTime(2026, 6, 9), "Come watch some of Cape Towns funniest performers that will make you cry with laughter.", "Baxter Theatre", "/EventImages/Funny.jpg");
            AddEvent("Suidoosterfees", "Culture and Creativity", new DateTime(2026, 4, 15), "Come see a variety of live musicals, performances and art exhibitions.", "Artscape Theatre", "/EventImages/Suid.jpg");

            //Business and Innovation events
            AddEvent("Africa Travel Week", "Business and innovation", new DateTime(2026, 3, 21), "Africa's best tourism trade show.", "Cape Town International Convention Centre", "/EventImages/Travel.jpg");
            AddEvent("Design Indaba", "Business and innovation", new DateTime(2026, 2, 13), "Cape Towns anual festival to celebrate creativity across design fields.", "Artscape Theatre", "/EventImages/Design.jpg");
            
            //Community driven events
            AddEvent("Jazzathon", "Community driven events", new DateTime(2026, 1, 23), "An anual event that includes live performances, workshops and productions.", "GrandWest Casino", "/EventImages/Jazzathon.jpg");
            
            //PLEASE NOTE: These events are from the Cape Town Municipal Website
            //https://www.capetown.gov.za/Explore%20and%20enjoy/Events-in-the-city/City-supported-events/Signature-Cape-Town-events
        }

        /// <summary>
        /// Method used to create an event and add it to the list of events
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="date"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="imgPath"></param>
        private static void AddEvent(string name, string category, DateTime date, string description, string location, string imgPath)
        {
            //Creating a new event with the attributes
            Event newEvent = new Event(name, category, description, location, date, imgPath);

            if (!eventsSortedDate.ContainsKey(date))
            {
                eventsSortedDate[date] = new List<Event>();
            }

            //Adding a new event to the sorted events
            eventsSortedDate[date].Add(newEvent);

            //Adding the category to the Hashet of categories
            categories.Add(category);
        }


        /// <summary>
        /// Method that handles the functionality of the events page(Searching, sorting and dislaying events and announcments) 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="categorySearch"></param>
        /// <param name="sortOrder"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Events(string search = "", string categorySearch = "", string sortOrder = "", string date = "")
        {

            var eventsList = eventsSortedDate.SelectMany(e => e.Value).ToList();
            DateTime? selectedDate = null;

            //Search by category implementation
            if (!string.IsNullOrEmpty(categorySearch))
            {
               eventsList = eventsList.Where(e => e.Category == categorySearch).ToList();
            }

            //Search by event name implementation
            if (!string.IsNullOrEmpty(search))
            {
                eventsList = eventsList.Where(e => e.eventName.ToLower().Contains(search.ToLower())).ToList();
            }

            //Search by date implementation
            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime dt))
            {
                selectedDate = dt.Date;
                eventsList = eventsList.Where(e => e.Date.Date == selectedDate.Value).ToList();
            }
            //Sorting implementation
            eventsList = sortOrder switch
            {
                "dateAscending" => eventsList.OrderBy(e => e.Date).ToList(),
                "dateDescending" => eventsList.OrderByDescending(e => e.Date).ToList(),
                "nameAscending" => eventsList.OrderBy(e => e.eventName).ToList(),
                "nameDescending" => eventsList.OrderByDescending(e => e.eventName).ToList(),
                "categoryAscendng" => eventsList.OrderBy(e => e.Category).ToList(),
                "categoryDescending" => eventsList.OrderByDescending(e => e.Category).ToList(),
                _ => eventsList.OrderBy(e => e.Date).ToList()
            };

            //Suggested events based on previous category and date searches
            var suggestedEvents = eventsSortedDate
            .SelectMany(e => e.Value)
            .Where(ev => (!string.IsNullOrEmpty(categorySearch) && ev.Category == categorySearch) || (selectedDate.HasValue && ev.Date.Date == selectedDate.Value) ||
             userSearches.Any(searchTerm => ev.Category.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
             ev.Date.ToString("yyyy-MM-dd") == searchTerm))
            .Where(ev => !recentlyViewedEvents.Any(rv => rv.eventName == ev.eventName && rv.Date.Date == ev.Date.Date))
            .OrderBy(ev => ev.Date)
            .Take(5)
            .ToList();

            //Tracking the category search after creating suggested events
            if (!string.IsNullOrEmpty(categorySearch))
            {
                TrackUserSearch(categorySearch);
            }
            //Tracking the date search after creating suggested events
            if (selectedDate.HasValue) 
            { 
                TrackUserSearch(selectedDate.Value.ToString("yyyy-MM-dd"));
            }

            //Creating announcements to dispaly on the webpage
            var announcements = new List<Announcement>
            {
                new Announcement { name = "Road works", description = "Road maintenance on 33 Boundry road", Date = DateTime.Now.AddDays(8) },
                new Announcement { name = "Apartments", description = "New apartment block being built in Observatory", Date = DateTime.Now.AddDays(54) },
                new Announcement { name = "New Electricity regulations", description = "Electricity regulations are active from Friday", Date = DateTime.Now.AddDays(3) },

            };

            //Passing the data to the view
            ViewBag.Categories = categories.OrderBy(c => c).ToList();
            ViewBag.RecentlyViewed = recentlyViewedEvents.ToList();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = categorySearch;
            ViewBag.CurrentDate = date;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SuggestedEvents = suggestedEvents;
            ViewBag.Announcements = announcements;

            return View(eventsList);
        }


        /// <summary>
        /// Method that adds the event that the user viewed to the recently viewed events
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult ViewEvent(string eventName)
        {
            Event evt = eventsSortedDate.SelectMany(e => e.Value).FirstOrDefault(ev => ev.eventName == eventName);

            if (evt != null)
            {
                //Retreiving the viewed event and putting it into the recently viewed events
                recentlyViewedEvents = new Stack<Event>(recentlyViewedEvents.Where(e => e.eventName != eventName));
                recentlyViewedEvents.Push(evt);

                //Only displaying up to 5 recently viewed events
                if (recentlyViewedEvents.Count > 5)
                {
                    recentlyViewedEvents = new Stack<Event>(recentlyViewedEvents.Take(5).Reverse());
                }
            }

            return RedirectToAction("Events");
        }

        /// <summary>
        /// Helper method used to track the user search either by category or date
        /// </summary>
        /// <param name="search"></param>
        private static void TrackUserSearch(string search)
        {
            //If the input is not empty then it will run
            if (!string.IsNullOrEmpty(search))
            {
                //Making the input lowercase
                search = search.ToLower();
                
                //If the new search is has not already been searched it will be added to userSearches
                if (!userSearches.Contains(search))
                {
                    //Adding it to  userSearches
                    userSearches.Insert(0, search);
                    //Only using last 10 searches of the user
                    if (userSearches.Count > 10)
                    {
                        userSearches.RemoveAt(userSearches.Count - 1);
                    }
                }
            }
        }
    }
}
//Reference:
//OpenAI, 2025. ChatGPT (Version GPT-5 mini).[Large language model].Available at: https://chatgpt.com/c/68cd30d8-86c4-8323-866a-e2bc1d538387 [Accessed: 13 October 2025]. 