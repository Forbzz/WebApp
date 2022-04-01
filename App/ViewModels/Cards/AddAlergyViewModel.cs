using Data.Domain;

namespace App.ViewModels.Cards
{
    public class AddAlergyViewModel
    {
        public string ReturnUrl { get; set; }
        public string PacientId { get; set; }
        public int CardId { get; set; }
        public Allergy Allergy { get; set; }

    }
}
