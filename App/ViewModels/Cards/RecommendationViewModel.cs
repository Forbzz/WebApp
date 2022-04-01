using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Cards
{
    public class RecommendationViewModel
    {
        public string ReturnUrl { get; set; }
        public string PacientId { get; set; }
        public string Search { get; set; }
        public IEnumerable<Recommendation> Reccomendations { get; set; }

    }
}
