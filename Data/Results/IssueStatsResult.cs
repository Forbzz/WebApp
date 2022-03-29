using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Results
{
    public class IssueStatsResult
    {
        public IEnumerable<IssueStatsResultItem> DayStats { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class IssueStatsResultItem
    {
        public string IssueName { get; set; }
        public int Count { get; set; }
    }
}
