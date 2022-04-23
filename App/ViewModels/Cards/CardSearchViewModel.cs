using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Cards
{
    public class CardSearchViewModel
    {
        public string Search { get; set; }
        public Pacient Pacient { get; set; }
        public Card Card { get; set; }
        public string ReturnUrl { get; set; }
        public int AllrgeysCount { get; set; }
        public int ExaminationsCount { get; set; }
        public int VaccinationsCount { get; set; }
        public int DiagnosesCount { get; set; }
        public int DiagnoseHistoryCount { get; set; }
        public int ReccomendationsCount { get; set; }
        public IEnumerable<DiagnoseHistory> DiagnoseHistories { get; set; }
    }
}
