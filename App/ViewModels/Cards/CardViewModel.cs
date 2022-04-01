using Data.Domain;

namespace App.ViewModels.Cards
{
    public class CardViewModel
    {
        public string ReturnUrl { get; set; }
        public Card Card { get; set; }
        public Pacient Pacient { get; set; }
    }
}
