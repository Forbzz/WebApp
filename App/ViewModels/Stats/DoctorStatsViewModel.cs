using Data.Results;
using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Stats
{
    public class DoctorStatsViewModel
    {
        public string Id { get; set; }

        public DoctorStatsResult Stats { get; set; }

        public string ReturnUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Начало")]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Конец")]
        public DateTime End { get; set; }
    }
}
