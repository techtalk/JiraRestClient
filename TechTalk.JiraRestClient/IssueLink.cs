namespace TechTalk.JiraRestClient
{
    public class IssueLink
    {
        public IssueLink()
        {
            type = new LinkType();
            inwardIssue = new IssueRef();
            outwardIssue = new IssueRef();
        }

        public string id { get; set; }

        public LinkType type { get; set; }
        public IssueRef outwardIssue { get; set; }
        public IssueRef inwardIssue { get; set; }
    }

    public class LinkType
    {
        public string name { get; set; }
    }
}
