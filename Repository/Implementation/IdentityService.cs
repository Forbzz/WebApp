using Data;
using Data.Domain;
using Data.Domain.HomeInfo;
using Data.Results;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Repository.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private ILogger<IdentityService> _logger;

        public IdentityService(UserManager<AppUser> userManager, ApplicationDbContext dbContext, ILogger<IdentityService> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateUser<T>(string mail, string password, string role) where T : AppUser, new()
        {
            var user = new T() { UserName = mail, Email = mail, Address = new Address(), Contacts = new Contacts() };

            var res = await _userManager.CreateAsync(user, password);
            if (res.Succeeded)
            {
                var a = await _userManager.AddToRoleAsync(user, role);

                if (role == "Pacient")
                {
                    var pacient = await _dbContext.Pacients.FirstOrDefaultAsync(x => x.Id == user.Id);
                    pacient.Card = new Card();
                    await _dbContext.SaveChangesAsync();
                }
                if (role == "Doctor")
                {
                    var doc = await _dbContext.Doctors.FirstOrDefaultAsync(x => x.Id == user.Id);
                    doc.Cabinet = new Cabinet();
                    await _dbContext.SaveChangesAsync();
                }
                _logger.LogInformation("User created a new account with password.");
            }
            return res;
        }

        public IQueryable<GetUserResult> GetUsers()
        {
            var users = (from user in _userManager.Users
                         join role in _dbContext.UserRoles on user.Id equals role.UserId
                         select new GetUserResult
                         {
                             Email = user.Email,
                             Id = user.Id,
                             Name1 = user.Name1,
                             Name2 = user.Name2,
                             Name3 = user.Name3,
                             Role = _dbContext.Roles.FirstOrDefault(x => x.Id == role.RoleId).Name
                         });

            return users;
        }

    }
}

