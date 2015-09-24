using System;

namespace TechTalk.JiraRestClient
{
    public class ProjectVersion
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public bool archived { get; set; }
        public bool released { get; set; }
        public DateTime? releaseDate { get; set; }
        public bool overdue { get; set; }
        public int projectid { get; set; }
    }
}
