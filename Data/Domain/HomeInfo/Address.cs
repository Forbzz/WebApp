using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.HomeInfo
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Страна")]
        public string Country { get; set; }

        //[Display(Name = "Область")]
        //public string Region { get; set; }

        [Display(Name = "Район")]
        public string District { get; set; }

        [Display(Name = "Город")]
        public string Town { get; set; }

        public TownType TownType { get; set; }

        [Display(Name = "Улица")]
        public string Street { get; set; }

        public StreetType StreetType { get; set; }

        [Display(Name = "Номер дома")]
        public string HomeNumber { get; set; }

        [Display(Name = "Номер квартиры")]
        public string ApartmentNumber { get; set; }

        public AppUser User { get; set; }
    }
}

