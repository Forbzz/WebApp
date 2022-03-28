using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Examination
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата осмотра")]
        public DateTime ExaminationDate { get; set; }

        [Display(Name = "Доктор")]
        public Doctor Doctor { get; set; }

        [Display(Name = "Тип исследования")]
        public string Type { get; set; }

        [Display(Name = "Результат")]
        public string Result { get; set; }

        public Card Card { get; set; }
    }
}
