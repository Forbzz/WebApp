using Data.Domain;
using Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IScheduleService
    {
        IQueryable<Schedule> GetScheduleById(int id);
        Task<IEnumerable<FreeTicketsCountResult>> GetFreeTicketsCount(string doctorId, DateTime start, DateTime end);
        Task<IEnumerable<Schedule>> GetFreeTickets(string doctorId, DateTime date);
        Task<bool> SignTicket(Pacient pacient, int scheduleId, DateTime date);
        IQueryable<Ticket> GetUserTickets(bool isFuture, string userId = null, string docId = null);
        Task<bool> DeleteTicket(int id);
        Task<bool> IsSignedTicket(int scheduleId, DateTime date);
        Task CloseTicket(int id);
    }
}
