using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Cards
{
    public class DiagnoseHistoryViewModel
    {
        public string ReturnUrl { get; set; }
        public string PacientId { get; set; }
        public int? DiagnoseId { get; set; }
        public Diagnose Diagnose { get; set; }
        public string Search { get; set; }
        public IEnumerable<DiagnoseHistory> History { get; set; }

    }
}
