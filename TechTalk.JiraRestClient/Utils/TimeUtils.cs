using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechTalk.JiraRestClient.Utils
{
    public static class TimeUtils
    {
        public static readonly DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            return unixStart.AddMilliseconds(unixTime);
        }

        public static long ToUnixTime(DateTime time)
        {
            return (long)(time.ToUniversalTime() - unixStart).TotalMilliseconds;
        }
    }
}
