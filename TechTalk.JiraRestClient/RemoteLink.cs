namespace TechTalk.JiraRestClient
{
    public class RemoteLink
    {
        public string id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string summary { get; set; }

        internal static RemoteLink Convert(RemoteLinkResult result)
        {
            result.@object.id = result.id;
            return result.@object;
        }
    }
}
