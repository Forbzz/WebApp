using Data.Domain;

namespace App.ViewModels.Cards
{
    public class AddDiagnoseViewModel
    {
        public string ReturnUrl { get; set; }
        public Diagnose Diagnose { get; set; }
        public string PacientId { get; set; }
        public DiagnoseHistory History { get; set; }

    }
}
