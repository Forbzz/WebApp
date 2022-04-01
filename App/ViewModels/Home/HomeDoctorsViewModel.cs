using Data.Domain;
using System.Collections.Generic;

namespace App.ViewModels.Home
{
    public class HomeDoctorsViewModel
    {
        public IEnumerable<Doctor> Doctors { get; set; }
    }
}
