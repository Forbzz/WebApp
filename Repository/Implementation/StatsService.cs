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
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _dbContext;

        public StatsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DoctorStatsResult> GetDoctorStats(string Id, DateTime start, DateTime end)
        {
            var result = new DoctorStatsResult
            {
                StartDate = start,
                EndTime = end,
                DayStats = await GetDayStats(Id, start, end),
                TotalSchedules = await GetTotalSchedules(Id, start, end),
                TotalTickets = await GetTotalTickets(Id, start, end)
            };

            return result;
        }

        private async Task<int> GetTotalTickets(string Id, DateTime start, DateTime end)
        {
            var query = _dbContext.Tickets
                .Where(x => x.Schedule.Doctor.Id == Id
                            && (x.TicketDate >= start && x.TicketDate <= end));
            return await query.CountAsync();
        }

        private async Task<int> GetTotalSchedules(string Id, DateTime start, DateTime end)
        {
            int count = 0;
            for (DateTime i = start; i <= end; i += TimeSpan.FromDays(1))
            {
                var query = _dbContext.Schedules.Where(x => x.Doctor.Id == Id
                                                            && x.DayOfWeek == i.DayOfWeek);
                count += await query.CountAsync();
            }

            return count;
        }
        private async Task<IEnumerable<DoctorStatsResultItem>> GetDayStats(string Id, DateTime start, DateTime end)
        {
            var res = new List<DoctorStatsResultItem>();
            for (DateTime i = start; i <= end; i += TimeSpan.FromDays(1))
            {
                var item = new DoctorStatsResultItem()
                {
                    Date = i,
                    ScheduleCount = await _dbContext.Schedules
                        .Where(x => x.Doctor.Id == Id && x.DayOfWeek == i.DayOfWeek)
                        .CountAsync(),
                    TicketsCount = await _dbContext.Tickets.Where(x => x.Schedule.Doctor.Id == Id
                                                                       && x.TicketDate.Date == i.Date)
                        .CountAsync()
                };
                res.Add(item);
            }

            return res;
        }
        public async Task<IssueStatsResult> GetIssueStats(DateTime start, DateTime end)
        {
            var result = new IssueStatsResult();
            result.Start = start;
            result.End = end;
            result.DayStats = await (from p in _dbContext.Diagnoses
                                     group p by p.Name
                into g
                                     select new IssueStatsResultItem()
                                     {
                                         Count = g.Count(),
                                         IssueName = g.Key
                                     }).ToArrayAsync();
            return result;
        }
    }
}
