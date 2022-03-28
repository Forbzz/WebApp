using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Recommendation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Реккомендация")]
        public string Descripton { get; set; }

        [Display(Name = "Начало")]
        public DateTime Start { get; set; }

        [Display(Name = "Конец")]
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }
        public Doctor Doctor { get; set; }
        public Card Card { get; set; }
    }
}
