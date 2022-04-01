using Data.Results;
using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Stats
{
    public class DiagnoseStatsViewModel
    {
        public string ReturnUrl { get; set; }

        public IssueStatsResult Stats { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Начало")]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Конец")]
        public DateTime End { get; set; }
    }
}
