using System;
using System.Globalization;

namespace TechTalk.JiraRestClient
{
    public class Timetracking
    {
        public string originalEstimate { get; set; }
        public int originalEstimateSeconds { get; set; }
        public int remainingEstimateSeconds { get; set; }
        public int timeSpentSeconds { get; set; }

        private const decimal DayToSecFactor = 8 * 3600;
        public decimal originalEstimateDays
        {
            get
            {
                return (decimal)originalEstimateSeconds / DayToSecFactor;
            }
            set
            {
                originalEstimate = string.Format(CultureInfo.InvariantCulture, "{0}d", value);
                originalEstimateSeconds = (int)(value * DayToSecFactor);
            }
        }
    }
}
