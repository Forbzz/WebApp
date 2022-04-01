using Data.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.User
{
    public class EditUserViewModel
    {
        public EditUserViewModel() { }
        
        public EditUserViewModel(AppUser user, string role)
        {
            Id = user.Id;
            Role = role;
            Name1 = user.Name1;
            Name2 = user.Name2;
            Name3 = user.Name3;
        }

        public EditUserViewModel(Doctor doc, string role, IEnumerable<string> branches,
            IEnumerable<string> specs) : this(doc as AppUser, role)
        {
            Branch = doc.Branch?.Name;
            Speciality = doc.Speciality?.Name;
            Branches = branches;
            Specialities = specs;
            CabinetNumber = doc?.Cabinet?.Number;
            ContactEmail = doc?.Contacts?.Mail;
            ContactPhoneNumber = doc?.Contacts?.Phone;
        }

        public string Id { get; set; }

        public string Role { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name1 { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Name2 { get; set; }

        [Display(Name = "Отчество")]
        public string Name3 { get; set; }

        [Display(Name = "Контактный номер")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Контактная почта")]
        public string ContactEmail { get; set; }

        [Display(Name = "Отделение")]
        public string Branch { get; set; }

        public IEnumerable<string> Branches { set; get; }

        [Display(Name = "Специальность")]
        public string Speciality { get; set; }

        public IEnumerable<string> Specialities { get; set; }

        public int? CabinetNumber { get; set; }

        public string returnUrl { get; set; }
    }
}
