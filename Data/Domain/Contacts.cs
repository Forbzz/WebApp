using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        //public string UserID { get; set; }

        //[ForeignKey(nameof(UserID))]
        public AppUser User { get; set; }
    }
}
