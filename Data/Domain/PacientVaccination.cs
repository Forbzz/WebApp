using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class PacientVaccination
    {
        [Key]
        public int Id { get; set; }
        public Card Card { get; set; }
        public Vaccination Vaccination { get; set; }

        [Display(Name = "Результат")]
        public string Result { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
    }
}
