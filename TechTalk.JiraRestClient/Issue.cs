using System;
using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    public class Issue : IssueRef
    {
        public Issue() { fields = new IssueFields(); }

        public string expand { get; set; }

        public string self { get; set; }

        public IssueFields fields { get; set; }

        internal void ExpandLinks(Issue issue)
        {
            foreach (var link in issue.fields.issuelinks)
            {
                if (string.IsNullOrEmpty(link.inwardIssue.id))
                {
                    link.inwardIssue.id = issue.id;
                    link.inwardIssue.key = issue.key;
                }
                if (string.IsNullOrEmpty(link.outwardIssue.id))
                {
                    link.outwardIssue.id = issue.id;
                    link.outwardIssue.key = issue.key;
                }
            }
        }
    }
}
