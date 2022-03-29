using Data.Domain;
using Data.Domain.HomeInfo;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUserService
    {
        Task<Address> GetUserAddress(string userId);
        Task EditUserAddress(string userId, string apartmentNumber, string country, string district, string homeNumber,
            int? postalCode, string region, string street, string town);
        Task<IdentityResult> DeletePacient(AppUser user);
        Task<IdentityResult> DeleteDoctor(AppUser user);
        Task<IdentityResult> DeleteAdmin(AppUser user);
    }
}
