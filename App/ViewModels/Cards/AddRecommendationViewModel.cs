using Data.Domain;

namespace App.ViewModels.Cards
{
    public class AddRecommendationViewModel
    {
        public string ReturnUrl { get; set; }
        public string PacientId { get; set; }
        public Recommendation Recommendation { get; set; }

    }
}
