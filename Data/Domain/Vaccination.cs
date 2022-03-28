using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Vaccination
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата выпуска")]
        public DateTime Date { get; set; }

        public DateTime ExpriationDate { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }
        public bool IsActive { get; set; }
    }
}
