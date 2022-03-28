using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class DiagnoseHistory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Заключение")]
        public string Conclusion { get; set; }

        [Display(Name = "Дата")]
        public DateTime ConclusionDate { get; set; }

        public Doctor Doctor { get; set; }

        public Diagnose Diagnose { get; set; }

    }
}
