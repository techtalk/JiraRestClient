using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TechTalk.JiraRestClient
{
    public class AvatarUrls
    {
        [DeserializeAs(Name = "16x16")]
        public string sixteen { get; set; }
        [DeserializeAs(Name = "24x24")]
        public string twentyfour { get; set; }
        [DeserializeAs(Name = "32x32")]
        public string thirtytwo { get; set; }
        [DeserializeAs(Name = "48x48")]
        public string fourteigth { get; set; }
    }
}
