using Data;
using Data.Domain;
using Data.Domain.HomeInfo;
using Data.Results;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public UserService(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task EditUserAddress(string userId, string apartmentNumber, string country,
            string district, string homeNumber, int? postalCode, string region, string street, string town)
        {
            var address = await _dbContext.Addresses.Where(x => x.User.Id == userId)
                .Include(x => x.Country).Include(x => x.District).Include(x => x.Region)
                .Include(x => x.Street).Include(x => x.Town).FirstOrDefaultAsync();
            address.ApartmentNumber = apartmentNumber;
            address.HomeNumber = homeNumber;
            if (address?.Country?.FullName != country)
                address.Country = await AddCountry(country);
            if (address?.Region?.Name != region)
                address.Region = await AddRegion(region);
            if (address?.District?.Name != district)
                address.District = await AddDistrict(district);
            if (address?.Town?.Name != town)
                address.Town = await AddTown(town);
            if (address?.Street?.Name != street)
                address.Street = await AddStreet(street);
            _dbContext.Addresses.Update(address);
            await _dbContext.SaveChangesAsync();
        }
        private async Task<Country> AddCountry(string name)
        {
            var country = await _dbContext.Countries.FirstOrDefaultAsync(x => x.FullName == name);
            if (country == null)
            {
                country = new Country { FullName = name };
                await _dbContext.Countries.AddAsync(country);
                await _dbContext.SaveChangesAsync();
            }
            return country;
        }
        private async Task<Region> AddRegion(string name)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Name == name);
            if (region == null)
            {
                region = new Region { Name = name };
                await _dbContext.Regions.AddAsync(region);
                await _dbContext.SaveChangesAsync();
            }
            return region;
        }
        private async Task<District> AddDistrict(string name)
        {
            var district = await _dbContext.Districts.FirstOrDefaultAsync(x => x.Name == name);
            if (district == null)
            {
                district = new District { Name = name };
                await _dbContext.Districts.AddAsync(district);
                await _dbContext.SaveChangesAsync();
            }
            return district;
        }
        private async Task<Town> AddTown(string name)
        {
            var town = await _dbContext.Towns.FirstOrDefaultAsync(x => x.Name == name);
            if (town == null)
            {
                town = new Town { Name = name };
                await _dbContext.Towns.AddAsync(town);
                await _dbContext.SaveChangesAsync();
            }
            return town;
        }
        private async Task<Street> AddStreet(string name)
        {
            var street = await _dbContext.Streets.FirstOrDefaultAsync(x => x.Name == name);
            if (street == null)
            {
                street = new Street { Name = name };
                await _dbContext.Streets.AddAsync(street);
                await _dbContext.SaveChangesAsync();
            }
            return street;
        }

        public async Task<Address> GetUserAddress(string userId)
        {
            var adress = await _dbContext.Addresses.Where(x => x.User.Id == userId)
                .Include(x => x.Country).Include(x => x.District).Include(x => x.Region)
                .Include(x => x.Street).Include(x => x.Town).FirstOrDefaultAsync();
            return adress;
        }

        public async Task<IdentityResult> DeletePacient(AppUser user)
        {
            try
            {
                var pacient = await _userManager.FindByIdAsync(user.Id) as Pacient;

                return await _userManager.DeleteAsync(pacient);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return IdentityResult.Failed(new IdentityError { Description = e.Message });
            }
        }

        public async Task<IdentityResult> DeleteDoctor(AppUser user)
        {
            try
            {
                var doctor = await _userManager.FindByIdAsync(user.Id) as Doctor;

                var tickets = await _dbContext.Tickets.Where(x => x.Schedule.Doctor.Id == user.Id)
                    .ToArrayAsync();
                _dbContext.Tickets.RemoveRange(tickets);
                await _dbContext.SaveChangesAsync();

                return await _userManager.DeleteAsync(doctor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAdmin(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
