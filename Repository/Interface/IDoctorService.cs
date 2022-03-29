using Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IDoctorService
    {
        IQueryable<Doctor> GetDoctorsBySpec(int specId);
        IQueryable<Speciality> GetSpecialities();
        IQueryable<Doctor> GetDoctors(string name = null, string spec = null, string branch = null);
        IQueryable<Branch> GetBranches();
        IQueryable<Vaccination> GetVaccinations();
        Branch GetBranch(string name);
        Speciality GetSpeciality(string name);
        Task<Doctor> GetDoctorByIdAsync(string Id);
        Task<bool> DeleteAsync(string Id);
        Task<bool> UpdateAsync(string Id, string name1, string name2, string name3, string branch, string spec, string mail, string phone, int? cabinetNumber);
        IQueryable<Schedule> GetScheduleByDocId(string Id);
        Task<Schedule> GetScheduleById(int id);
        Task AddToSchedules(string docId, Schedule schedule);
        Task<bool> UpdateSchedule(Schedule schedule);
        Task<bool> DeleteScheduleAsync(int id);

    }
}
