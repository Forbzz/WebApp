using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Pacients
{

    public class PacientIndexViewModel
    {
        public PacientSearchViewModel Search { get; set; }
        public IEnumerable<Pacient> Pacients { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
    public class PacientSearchViewModel
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
    }

}
