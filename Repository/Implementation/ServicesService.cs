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
    public class ServicesService : IServicesService
    {
        private readonly ApplicationDbContext _dbContext;

        public ServicesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddVaccination(Vaccination vaccination)
        {
            var vc = await _dbContext.Vaccinations.Where(x => x.Type == vaccination.Type).FirstOrDefaultAsync();
            if (vc == null)
            {
                _dbContext.Vaccinations.Add(vaccination);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteVaccination(int Id)
        {
            var vc = await _dbContext.Vaccinations.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (vc != null)
            {
                vc.ExpriationDate = DateTime.Now.Date;
                vc.IsActive = false;
            }
        }
        public IQueryable<Vaccination> GetVaccinations(bool includeAll)
        {
            var query = _dbContext.Vaccinations.AsQueryable();
            if (includeAll)
            {
                return query;
            }
            return query.Where(x => x.IsActive);
        }
    }
}

