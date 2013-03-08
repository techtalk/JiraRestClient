using System;
using System.Collections.Generic;

namespace TechTalk.JiraRestClient
{
    public class CommentsContainer
    {
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<Comment> comments { get; set; }
    }
}
