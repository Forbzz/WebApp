using Data.Domain;

namespace App.ViewModels.Cards
{
    public class AddExaminationViewModel
    {
        public Examination Examination { get; set; }
        public string ReturnUrl { get; set; }
        public string PacientId { get; set; }

    }
}
