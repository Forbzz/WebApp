using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace App.ViewModels.User
{
    //???
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string Role { get; set; }
    }
}
