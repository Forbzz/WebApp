using Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IServicesService
    {
        IQueryable<Vaccination> GetVaccinations(bool includeAll);
        Task AddVaccination(Vaccination vaccination);
        Task DeleteVaccination(int In);
    }
}
