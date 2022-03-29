using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Results
{
    public class DoctorStatsResult
    {
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }

        [Display(Name = "Всего посещений")]
        public int TotalTickets { get; set; }

        [Display(Name = "Всего талонов")]
        public int TotalSchedules { get; set; }
        public TimeSpan AvgTicketTime { get; set; }
        public IEnumerable<DoctorStatsResultItem> DayStats { get; set; }

    }

    public class DoctorStatsResultItem
    {
        public DateTime Date { get; set; }
        public int TicketsCount { get; set; }
        public int ScheduleCount { get; set; }
    }
}
