using Data.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Cards
{
    public class AddVaccinationViewModel
    {
        public IEnumerable<Vaccination> Vaccinations { get; set; }
        public string ReturnUrl { get; set; }
        public string PacientId { get; set; }
        public int Vaccination { get; set; }

        [Required]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Результат")]
        public string Result { get; set; }

    }
}
