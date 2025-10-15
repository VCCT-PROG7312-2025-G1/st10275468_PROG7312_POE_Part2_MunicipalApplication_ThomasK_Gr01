namespace Municipal_Services_Portal.Models
{
    public class Event
    {
        public string eventName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string imgPath { get; set; }

        /// <summary>
        /// Constructor to initilize events
        /// </summary>
        /// <param name="eventname"></param>
        /// <param name="category"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="date"></param>
        /// <param name="imgpath"></param>
        public Event(string eventname, string category, string description, string location, DateTime date, string imgpath)
        {
            eventName = eventname;
            Category = category;
            Description = description;
            Location = location;
            Date = date;
            imgPath = imgpath;
        }
    }
}
