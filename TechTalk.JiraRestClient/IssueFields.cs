using System;
using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    public class IssueFields
    {
        public IssueFields()
        {
            status = new Status();
            timetracking = new Timetracking();

            labels = new List<String>();
            comments = new List<Comment>();
            issuelinks = new List<IssueLink>();
            attachment = new List<Attachment>();
            watchers = new List<JiraUser>();
        }

        public String summary { get; set; }
        public String description { get; set; }
        public Timetracking timetracking { get; set; }
        public Status status { get; set; }

        public JiraUser reporter { get; set; }
        public JiraUser assignee { get; set; }
        public List<JiraUser> watchers { get; set; } 

        public List<String> labels { get; set; }
        public List<Comment> comments { get; set; }
        public List<IssueLink> issuelinks { get; set; }
        public List<Attachment> attachment { get; set; }

        /// <summary>remaining time estimate in seconds</summary>
        public int? timeestimate { get; set; }
        /// <summary>original time estimate in seconds</summary>
        public int? timeoriginalestimate { get; set; }
        /// <summary>time logged in seconds</summary>
        public int? timespent { get; set; }

        public Resolution resolution { get; set; }
        public DateTime? resolutiondate { get; set; }

        public Priority priority { get; set; }
    }
}
