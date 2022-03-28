using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "День недели")]
        public DayOfWeek DayOfWeek { get; set; }

        [Display(Name = "Время")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Display(Name = "Продолжительность")]
        [DataType(DataType.Time)]
        public TimeSpan Duration { get; set; }
        public Doctor Doctor { get; set; }
    }
}
