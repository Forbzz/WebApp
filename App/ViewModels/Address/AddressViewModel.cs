using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels.Address
{
    public class AddressViewModel
    {
        [Required]
        [Display(Name = "Страна")]
        public string Country { get; set; }


        [Display(Name = "Область")]
        public string Region { get; set; }

        [Required]
        [Display(Name = "Район")]
        public string District { get; set; }

        [Required]
        [Display(Name = "Город")]
        public string Town { get; set; }

        [Required]
        [Display(Name = "Улица")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Номер дома")]
        [Range(1, 2147483647)]
        public string HomeNumber { get; set; }

        [Display(Name = "Номер квартиры")]
        public string ApartmentNumber { get; set; }


        [Display(Name = "Почтовый индекс")]
        [DataType(DataType.PostalCode)]
        public int? PostalCode { get; set; }

        public string UserId { get; set; }

        public string returnUrl { get; set; }
    }
}
