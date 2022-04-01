using Data.Domain;

namespace App.ViewModels.Cards
{
    public class AddDiagnoseHistory
    {
        public string ReurnUrl { get; set; }
        public int DiagnoseId { get; set; }
        public string PacientId { get; set; }
        public DiagnoseHistory History { get; set; }

    }
}
