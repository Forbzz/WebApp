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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _dbContext;
        public ScheduleService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteTicket(int id)
        {
            var ticket = new Ticket { Id = id };
            _dbContext.Attach(ticket);
            _dbContext.Remove(ticket);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<IEnumerable<Schedule>> GetFreeTickets(string doctorId, DateTime date)
        {
            var tikets = _dbContext.Tickets
                .Where(x => x.TicketDate.Date == date.Date)
                .Where(x => x.Schedule.Doctor.Id == doctorId)
                .Select(x => x.Schedule).ToList();

            var schedules = _dbContext.Schedules
                .Where(x => x.Doctor.Id == doctorId)
                .Where(x => x.DayOfWeek == date.DayOfWeek)
                .AsEnumerable();
            schedules = schedules
                .Except(tikets).Select(x => x)
                .ToList();


            return schedules;
        }

        public async Task<IEnumerable<FreeTicketsCountResult>> GetFreeTicketsCount(string doctorId, DateTime start, DateTime end)
        {
            var schCount = _dbContext.Schedules
                .Where(x => x.Doctor.Id == doctorId)
                .AsEnumerable()
                .GroupBy(x => x.DayOfWeek)
                .ToDictionary(x => x.Key, y => y.Count());
            //var schCount = await (from sch in _dbContext.Schedules
            //               group sch by sch.DayOfWeek into gr)
            //               .ToDictionaryAsync(x => x.Name, x => x.Count());
            var query = from ticket in _dbContext.Tickets
                        where ticket.Schedule.Doctor.Id == doctorId
                        where ticket.TicketDate >= start && ticket.TicketDate <= end
                        group ticket by ticket.TicketDate into gr
                        select new FreeTicketsCountResult { Start = gr.Key, Title = gr.Count() };
            var list = query.ToList();
            var res = new List<FreeTicketsCountResult>();
            for (DateTime i = start; i < end; i += TimeSpan.FromDays(1))
            {
                if (i.Date > DateTime.Now && schCount.ContainsKey(i.DayOfWeek))
                {
                    var daycount = schCount[i.DayOfWeek];
                    var talons1 = query.ToList();
                    var talons = query.Where(x => x.Start.Date == i.Date).Select(x => x.Title).FirstOrDefault();
                    res.Add(new FreeTicketsCountResult
                    {
                        Start = i - (i - i.Date),
                        End = i.Date + TimeSpan.FromHours(1),
                        Title = daycount - talons
                    });
                }

            }
            return res;
        }

        public IQueryable<Schedule> GetScheduleById(int id)
        {
            return _dbContext.Schedules.Where(x => x.Id == id);
        }

        public IQueryable<Ticket> GetUserTickets(bool isFuture, string userId = null, string docId = null)
        {
            var query = _dbContext.Tickets.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.Pacient.Id == userId);
            }
            if (docId != null)
            {
                query = query.Where(x => x.Schedule.Doctor.Id == docId);
            }
            if (isFuture)
            {
                query = query.Where(x => x.TicketDate.Date >= DateTime.Now && !x.Status);
            }
            return query;
        }

        public async Task<bool> IsSignedTicket(int scheduleId, DateTime date)
        {
            var ticket = await _dbContext.Tickets.Where(x => x.Schedule.Id == scheduleId &&
                x.TicketDate == date).FirstOrDefaultAsync();
            return ticket == null ? false : true;
        }

        public async Task CloseTicket(int id)
        {
            var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            if (ticket != null)
            {
                ticket.Status = true;
                _dbContext.Tickets.Update(ticket);
                var a = await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> SignTicket(Pacient pacient, int schedule, DateTime date)
        {
            var sch = _dbContext.Schedules.FirstOrDefault(x => x.Id == schedule);
            _dbContext.Tickets.Add(new Ticket
            {
                Pacient = pacient,
                Schedule = sch,
                TicketDate = date
            });
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}

