using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    [Table("Doctors")]
    public class Doctor : AppUser
    {
        [Display(Name = "Отделение")]
        public Branch Branch { get; set; }

        [Display(Name = "Специальность")]
        public Speciality Speciality { get; set; }

        [Display(Name = "Кабинет")]
        public Cabinet Cabinet { get; set; }

        public IEnumerable<Schedule> Schedule { get; set; }
        public IEnumerable<Recommendation> Recomemndations { get; set; }
        public IEnumerable<Examination> Examinations { get; set; }
        public IEnumerable<DiagnoseHistory> Histories { get; set; }
        public IEnumerable<Diagnose> EstablisheDiagnoses { get; set; }
        public IEnumerable<Diagnose> ConfirmedDiagnoses { get; set; }

    }
}
