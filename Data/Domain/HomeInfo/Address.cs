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
        public Country Country { get; set; }

        [Display(Name = "Область")]
        public Region Region { get; set; }

        [Display(Name = "Район")]
        public District District { get; set; }

        [Display(Name = "Город")]
        public Town Town { get; set; }

        public TownType TownType { get; set; }

        [Display(Name = "Улица")]
        public Street Street { get; set; }

        public StreetType StreetType { get; set; }

        [Display(Name = "Номер дома")]
        public string HomeNumber { get; set; }

        [Display(Name = "Номер квартиры")]
        public string ApartmentNumber { get; set; }

        public AppUser User { get; set; }
    }
}

