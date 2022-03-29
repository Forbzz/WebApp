using Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IStatsService
    {
        Task<DoctorStatsResult> GetDoctorStats(string Id, DateTime start, DateTime end);
        Task<IssueStatsResult> GetIssueStats(DateTime start, DateTime end);
    }
}
