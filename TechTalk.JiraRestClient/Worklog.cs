﻿using System;
using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    public class Worklog
    {
        public JiraUser author { get; set; }
        public JiraUser updateAuthor { get; set; }
        public string comment { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public DateTime started { get; set; }
        public string timeSpent { get; set; }
        public int timeSpentSeconds { get; set; }
        public string id { get; set; }
        public string issueId { get; set; }
    }
}
