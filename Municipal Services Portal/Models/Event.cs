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
