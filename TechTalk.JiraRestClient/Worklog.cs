using System;

namespace TechTalk.JiraRestClient
{
    public class Worklog
    {
        public string id { get; set; }
        public JiraUser author { get; set; }
        public JiraUser updateAuthor { get; set; }
        public string comment { get; set; }
        public DateTime started { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public int timeSpentSeconds { get; set; }
    }
}
