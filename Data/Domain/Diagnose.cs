using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Diagnose
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Диагноз")]
        public string Name { get; set; }

        public IEnumerable<DiagnoseHistory> DiagnoseHistorie { get; set; }

        [Display(Name = "Установил")]
        public Doctor DoctorEstablishe { get; set; }

        [Display(Name = "Дата установления")]
        public DateTime EstablisheDate { get; set; }

        [Display(Name = "Подтвердил")]
        public Doctor DoctorConfirm { get; set; }

        [Display(Name = "Дата подтверждения")]
        public DateTime? ConfirmDate { get; set; }

        public bool Status { get; set; }

        public Card Card { get; set; }

    }
}
