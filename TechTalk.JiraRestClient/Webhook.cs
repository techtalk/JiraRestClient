using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    public class Webhook
    {
        public string webhookEvent { get; set; }
        public JiraUser user { get; set; }
        public Issue issue { get; set; }
        public ChangeLog changelog { get; set; }
        public string timestamp { get; set; }
    }
    public class ChangeLog
    {
        public string id { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string field { get; set; }
        public string fieldtype { get; set; }
        public string from { get; set; }
        public string fromString { get; set; }
        public string to { get; set; }
        public string toString { get; set; }
    }
}
