using System;
using System.Collections.Generic;
using TechTalk.JiraRestClient.Utils;

namespace TechTalk.JiraRestClient
{
    public class WorklogUpdated
    {
        public List<WorklogUpdatedValue> values { get; set; }
        public long since { get; set; }
        public long until { get; set; }
        public bool lastPage { get; set; }

        public DateTime Since
        {
            get
            {
                return TimeUtils.FromUnixTime(since);
            }
        }

        public DateTime Until
        {
            get
            {
                return TimeUtils.FromUnixTime(until);
            }
        }
    }

    public class WorklogUpdatedValue
    {
        public int worklogId { get; set; }
        public long updatedTime { get; set; }

        public DateTime UpdatedTime
        {
            get
            {
                return TimeUtils.FromUnixTime(updatedTime);
            }
        }
    }
}
