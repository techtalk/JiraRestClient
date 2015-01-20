using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTalk.JiraRestClient
{
    internal class ChangelogContainer
    {
        public string expand { get; set; }

        public ChangeLog changelog { get; set; }
    }

    public class ChangeLog
    {
        public List<Change> histories { get; set; }
    }

    public class Change
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public List<ChangeItem> items { get; set; }
    }

    public class ChangeItem
    {
        public string field { get; set; }
        //public int? from { get; set; }
        public string fromString { get; set; }
        //public int? to { get; set; }
        public string toString { get; set; }
    }
}
