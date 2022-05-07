using Data.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Doctors
{
    public class DoctorScheduleViewModel
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Отделение")]
        public string Branch { get; set; }

        [Display(Name = "Специальность")]
        public string Speciality { get; set; }

        public ICollection<string> Branches { get; set; }

        public ICollection<string> Specialities { get; set; }

        public IEnumerable<Doctor> Doctors { get; set; }
    }
}
