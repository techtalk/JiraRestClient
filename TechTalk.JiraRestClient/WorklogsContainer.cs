using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    internal class WorklogsContainer
    {
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<Worklog> Worklogs { get; set; }
    }
}
