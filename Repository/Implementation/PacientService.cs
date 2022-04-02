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
    public class PacientService : IPacientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public PacientService(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<Pacient> GetPacientByIdAsync(string Id, bool includeContacts, bool includeAddress, bool includeCard)
        {
            var query = _dbContext.Pacients.Where(x => x.Id == Id);
            if (includeContacts)
                query = query.Include(x => x.Contacts);
            if (includeAddress)
                query = query.Include(x => x.Address);
                    //.Include(x => x.Address.Country)
                    ////.Include(x => x.Address.Region)
                    //.Include(x => x.Address.District)
                    //.Include(x => x.Address.Street)
                    //.Include(x => x.Address.Town);
            if (includeCard)
                query = query.Include(x => x.Card);
            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<Pacient> GetPacients(int skip = -1, int take = -1,
            bool includeContacts = false, bool includeAddress = false, bool includeCard = false)
        {
            var query = _dbContext.Pacients.AsQueryable();
            if (includeContacts)
            { query = query.Include(x => x.Contacts); }
            if (includeCard)
            {
                query = query.Include(x => x.Card).Select(x => x);
            }
            if (includeAddress)
            {
                query = query.Include(x => x.Address);
                    //.Include(x => x.Address.Country)
                    ////.Include(x => x.Address.Region)
                    //.Include(x => x.Address.District)
                    //.Include(x => x.Address.Street)
                    //.Include(x => x.Address.Town);
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            if (take > 0)
            {
                query = query.Take(take);
            }
            return query;
        }
        public async Task<Address> GetPacientAddress(string pacientId)
        {
            return await _dbContext.Addresses.Where(x => x.User.Id == pacientId).FirstOrDefaultAsync();
        }
        public IQueryable<Pacient> SearchPacient(string name, string cardNumber, string address,
            int skip = -1, int take = -1, bool includeCard = false,
            bool includeContacts = false, bool includeAddress = false)
        {
            var query = _dbContext.Pacients.AsQueryable();
            if (includeAddress)
            {
                query = query.Include(x => x.Address);
                    //.Include(x => x.Address.Country)
                    //.Include(x => x.Address.District)
                    ////.Include(x => x.Address.Region)
                    //.Include(x => x.Address.Street)
                    //.Include(x => x.Address.Town);
            }
            if (includeCard)
            {
                query = query.Include(x => x.Card);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(HasName(name));
            }
            if (!string.IsNullOrEmpty(cardNumber))
            {
                query = query.Where(x => x.Card.Number.ToString().StartsWith(cardNumber));
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(HasAddress(address));
            }
            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            if (take > 0)
            {
                query = query.Take(take);
            }



            return query.Select(x => x);
        }
        private Expression<Func<Pacient, bool>> HasName(string name)
        {
            var predicate = PredicateBuilder.New<Pacient>(true);

            if (!string.IsNullOrEmpty(name))
            {
                var names = name.Trim().ToLower().Split(' ');

                foreach (var n in names)
                {
                    predicate.Or(c => c.Name1.ToLower().Contains(n));
                    predicate.Or(c => c.Name2.ToLower().Contains(n));
                    predicate.Or(c => c.Name3.ToLower().Contains(n));
                }
            }

            return predicate;
        }
        private Expression<Func<Pacient, bool>> HasAddress(string address)
        {
            var predicate = PredicateBuilder.New<Pacient>(true);

            var addresses = address.Trim().ToLower().Split(' ');

            foreach (var ad in addresses)
            {
                //predicate.Or(x => x.Address.Country.FullName.Contains(ad));
                //predicate.Or(x => x.Address.District.Name.Contains(ad));
                //predicate.Or(x => x.Address.District.Name.Contains(ad));
                //predicate.Or(x => x.Address.Region.Name.Contains(ad));
                //predicate.Or(x => x.Address.Street.Name.Contains(ad));
                //predicate.Or(x => x.Address.Town.Name.Contains(ad));
                //predicate.Or(x => x.Address.HomeNumber.Contains(ad));

                predicate.Or(x => x.Address.Country.Contains(ad));
                predicate.Or(x => x.Address.District.Contains(ad));
                predicate.Or(x => x.Address.District.Contains(ad));
                predicate.Or(x => x.Address.Street.Contains(ad));
                predicate.Or(x => x.Address.Town.Contains(ad));
                predicate.Or(x => x.Address.HomeNumber.Contains(ad));
            }
            return predicate;
        }

        public async Task<bool> UpdateAsync(Pacient pacient)
        {
            var user = await _dbContext.Pacients.Where(x => x.Id == pacient.Id).Include(x => x.Contacts)
                .Include(x => x.Card)
                .FirstOrDefaultAsync();
            user.Name1 = pacient.Name1;
            user.Name2 = pacient.Name2;
            user.Name3 = pacient.Name3;
            user.Male = pacient.Male;
            user.BirthDay = pacient.BirthDay;
            user.Work = pacient.Work;
            user.Contacts.Mail = pacient.Contacts.Mail;
            user.Contacts.Phone = pacient.Contacts.Phone;
            user.Card.Number = pacient.Card.Number;
            _dbContext.Pacients.Update(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
