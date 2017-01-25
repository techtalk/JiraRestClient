﻿using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    public class Project
    {
        public string self { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string email { get; set; }
        public string assigneeType { get; set; }
        public JiraUser lead { get; set; }
        public List<IssueType> IssueTypes { get; set; }
        public List<Component> components { get; set; }
        public ProjectCategory projectCategory { get; set; }
    }
}