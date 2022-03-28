using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


namespace Data.Domain
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Последний вход")]
        public DateTime LastLoginDate { get; set; }

        [Display(Name = "Имя")]
        public string Name1 { get; set; }

        [Display(Name = "Фмалилия")]
        public string Name2 { get; set; }

        [Display(Name = "Отчество")]
        public string Name3 { get; set; }

    }
}
