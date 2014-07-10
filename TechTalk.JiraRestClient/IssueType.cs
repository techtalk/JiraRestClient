﻿namespace TechTalk.JiraRestClient
{
    public class IssueType
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public bool subtask { get; set; }
    }
}
