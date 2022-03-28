using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Passport
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Выдан")]
        public string Authority { get; set; }

        [Display(Name = "Пол")]
        public Gender Gender { get; set; }

        [Display(Name = "ИД номер")]
        public string Identity { get; set; }

        [Display(Name = "Дата выдачи")]
        public DateTime DateOfIssue { get; set; }

        [Display(Name = "Дата окончания")]
        public DateTime DateOfExpiry { get; set; }

    }
}
