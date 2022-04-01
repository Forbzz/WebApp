using Data.Domain;

namespace App.ViewModels.Cards
{
    public class AddDiagnoseHistoryViewModel
    {
        public string ReturnUrl { get; set; }
        public int DiagnoseId { get; set; }
        public DiagnoseHistory History { get; set; }

    }
}
