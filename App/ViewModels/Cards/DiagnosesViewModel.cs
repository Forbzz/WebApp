using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Cards
{
    public class DiagnosesViewModel
    {
        public IEnumerable<Diagnose> Diagnoses { get; set; }
        public string PacientId { get; set; }
        public string Search { get; set; }
        public string ReturnUrl { get; set; }

    }
}
