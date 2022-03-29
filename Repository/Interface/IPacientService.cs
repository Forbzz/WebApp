using Data.Domain;
using Data.Domain.HomeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IPacientService
    {
        Task<Pacient> GetPacientByIdAsync(string id, bool includeContacts, bool includeAddress, bool includeCard);
        IQueryable<Pacient> GetPacients(int skip = -1, int take = -1, bool includeContacts = false, bool includeAddress = false, bool includeCard = false);
        Task<Address> GetPacientAddress(string pacientId);
        IQueryable<Pacient> SearchPacient(string name, string cardNumber, string address, int skip = -1, int take = -1,
            bool includeContacts = false, bool includeAddress = false, bool includeCard = false);
        Task<bool> UpdateAsync(Pacient pacient);
    }
}
