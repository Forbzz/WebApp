using Data.Domain;

namespace App.ViewModels.Home
{
    public class EditScheduleViewModel
    {
        public string DocId { get; set; }
        public string returnUrl { get; set; }
        public Schedule Schedule { get; set; }
    }
}
