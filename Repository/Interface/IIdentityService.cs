using Data.Domain;
using Data.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IIdentityService
    {
        IQueryable<GetUserResult> GetUsers();
        Task<IdentityResult> CreateUser<T>(string email, string password, string role) where T : AppUser, new();
    }
}
