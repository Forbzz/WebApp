using Data;
using Data.Domain;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public DoctorService(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IQueryable<Doctor> GetDoctors(string name = null, string spec = null, string branch = null)
        {
            var query = _dbContext.Doctors
                .Where(HasName(name))
                .Include(x => x.Branch)
                .Where(x => string.IsNullOrEmpty(branch) || x.Branch.Name.Contains(branch))
                .Include(x => x.Speciality)
                .Where(x => string.IsNullOrEmpty(spec) || x.Speciality.Name.Contains(spec))
                .Select(x => x);
            return query;
        }

        public IQueryable<Doctor> GetDoctorsBySpec(int id)
        {
            var query = _dbContext.Doctors.Include(x => x.Speciality).Where(x => x.Speciality.Id == id)
                .Include(x => x.Cabinet).Select(x => x);
            return query;
        }
        public IQueryable<Speciality> GetSpecialities() => _dbContext.Specialities;
        public IQueryable<Branch> GetBranches() => _dbContext.Branches;

        public IQueryable<Vaccination> GetVaccinations()
        {
            return _dbContext.Vaccinations.AsQueryable<Vaccination>();
        }
        private Expression<Func<Doctor, bool>> HasName(string name)
        {
            var predicate = PredicateBuilder.New<Doctor>(true);

            if (!string.IsNullOrEmpty(name))
            {
                var names = name.Trim().ToLower().Split(' ');

                foreach (var n in names)
                {
                    predicate.Or(c => c.Name1.ToLower().Contains(n.ToLower()));
                    predicate.Or(c => c.Name2.ToLower().Contains(n.ToLower()));
                    predicate.Or(c => c.Name3.ToLower().Contains(n.ToLower()));
                }
            }

            return predicate;
        }

        public Branch GetBranch(string name) => _dbContext.Branches.FirstOrDefault(x => x.Name == name);

        public Speciality GetSpeciality(string name) => _dbContext.Specialities.FirstOrDefault(x => x.Name == name);

        public async Task<Doctor> GetDoctorByIdAsync(string Id)
        {
            var doc = await _dbContext.Doctors
                .Where(x => x.Id == Id)
                .Include(x => x.Branch)
                .Include(x => x.Speciality)
                .Include(x => x.Cabinet)
                .Include(x => x.Contacts)
                .FirstOrDefaultAsync();
            return doc;
        }

        public Task<bool> DeleteAsync(string Id)
        {
            throw new NotImplementedException(); //TODO delete
        }

        public async Task<bool> UpdateAsync(string Id, string name1, string name2, string name3,
            string branch, string spec, string mail, string phone, int? cabinetNumber)
        {
            var doctor = await _dbContext.Doctors.Where(x => x.Id == Id)
                .Include(x => x.Cabinet).Include(x => x.Contacts).FirstOrDefaultAsync();
            doctor.Name1 = name1;
            doctor.Name2 = name2;
            doctor.Name3 = name3;
            if (!string.IsNullOrEmpty(branch))
            {
                var Branch = await AddBranchAsync(branch);
                doctor.Branch = Branch;
            }
            if (!string.IsNullOrEmpty(spec))
            {
                var Spec = await AddSpecAsync(spec);
                doctor.Speciality = Spec;
            }
            var contacts = new Contacts
            {
                Mail = mail,
                Phone = phone
            };
            doctor.Contacts = contacts;
            var cabinet = new Cabinet
            {
                Number = cabinetNumber
            };
            doctor.Cabinet = cabinet;
            _dbContext.Doctors.Update(doctor);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        private async Task<Speciality> AddSpecAsync(string name, string descr = null)
        {
            var speciality = await _dbContext.Specialities.FirstOrDefaultAsync(x => x.Name == name);
            if (speciality is null)
            {
                speciality = new Speciality
                {
                    Name = name,
                    Description = descr
                };
                await _dbContext.Specialities.AddAsync(speciality);
            }
            return speciality;
        }

        public async Task<Branch> AddBranchAsync(string name, string descr = null)
        {
            var branch = await _dbContext.Branches.FirstOrDefaultAsync(x => x.Name == name);
            if (branch is null)
            {
                branch = new Branch
                {
                    Name = name,
                    Description = descr
                };
                await _dbContext.Branches.AddAsync(branch);
            }
            return branch;
        }

        public IQueryable<Schedule> GetScheduleByDocId(string Id)
        {
            return _dbContext.Schedules.Where(x => x.Doctor.Id == Id).Select(x => x);
        }
        public async Task AddToSchedules(string docId, Schedule schedule)
        {
            var doc = await GetDoctorByIdAsync(docId);
            schedule.Doctor = doc;
            _dbContext.Schedules.Add(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddToSchedulesRange(string docId, Schedule schedule, DayOfWeek[] days)
        {
            var doc = await GetDoctorByIdAsync(docId);
            schedule.Doctor = doc;
            var listSchedule = new List<Schedule>();
            foreach (var day in days)
            {

                listSchedule.Add(
                    new Schedule
                    {
                        Doctor = doc,
                        Time = schedule.Time,
                        Duration = schedule.Duration,
                        DayOfWeek = day

                    }
                );
            }
            _dbContext.Schedules.AddRange(listSchedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveFromSchedule(Schedule schedule)
        {
            _dbContext.Schedules.Remove(schedule);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Schedule> GetScheduleById(int id)
        {
            return await _dbContext.Schedules.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateSchedule(Schedule schedule)
        {
            _dbContext.Update(schedule);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var sch = await GetScheduleById(id);
            _dbContext.Remove(sch);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
