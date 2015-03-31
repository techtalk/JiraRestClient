using System;

namespace TechTalk.JiraRestClient
{
    public class IssueRef
    {
        public string id { get; set; }
        public string key { get; set; }

        internal string JiraIdentifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? key : id; }
        }
    }
}
