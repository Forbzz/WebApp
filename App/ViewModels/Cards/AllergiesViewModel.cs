using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Cards
{
    public class AllergiesViewModel
    {
        public string returnUrl { get; set; }
        public IEnumerable<Allergy> Allergies { get; set; }
        public string PacientId { get; set; }
        public int CardId { get; set; }
        public string Search { get; set; }

    }
}
