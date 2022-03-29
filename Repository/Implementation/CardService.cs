using Data;
using Data.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class CardService : ICardService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public CardService(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IQueryable<Allergy> GetAllergies(int cardId)
        {
            return _dbContext.Allergies.Where(x => x.Card.Id == cardId);
        }

        public async Task<Card> GetCardByIdAsync(string userId)
        {
            return await _dbContext.Cards.Where(x => x.PacientId == userId).FirstOrDefaultAsync();
        }

        public IQueryable<DiagnoseHistory> GetDiagnoseHistories(int cardId)
        {
            return _dbContext.DiagnoseHistories.Where(x => x.Diagnose.Card.Id == cardId);
        }
        public IQueryable<DiagnoseHistory> GetDiagnosesHistories(int diagnoseId)
        {
            return _dbContext.DiagnoseHistories.Where(x => x.Diagnose.Id == diagnoseId);
        }
        public IQueryable<Diagnose> GetDiagnoses(int cardId)
        {
            return _dbContext.Diagnoses.Where(x => x.Card.Id == cardId);
        }

        public IQueryable<Examination> GetExaminations(int cardId)
        {
            return _dbContext.Examinations.Where(x => x.Card.Id == cardId);
        }

        public IQueryable<PacientVaccination> GetPacientVaccinations(int cardId)
        {
            return _dbContext.PacientVaccinations.Where(x => x.Card.Id == cardId);
        }

        public IQueryable<Vaccination> GetVaccinations()
        {
            return _dbContext.Vaccinations.AsQueryable();
        }

        public IQueryable<Recommendation> GetReccomendations(int id)
        {
            return _dbContext.Recommendations.AsQueryable();
        }

        public IQueryable<Recommendation> SearchReccomendations(int cardId, string search)
        {
            return GetReccomendations(cardId).Where(x => x.Type.Contains(search) || x.Descripton.Contains(search));
        }

        public IQueryable<Allergy> SearchAllergies(int cardId, string search)
        {
            return GetAllergies(cardId).Where(x => x.Type.Contains(search));
        }

        public IQueryable<Diagnose> SearchDiagnoses(int cardId, string search)
        {
            return GetDiagnoses(cardId).Where(x => x.Name.Contains(search));
        }

        public IQueryable<DiagnoseHistory> SearchDiagnoseHistories(int cardId, string search)
        {
            return GetDiagnoseHistories(cardId)
                .Where(x => x.Diagnose.Name.Contains(search) || x.Conclusion.Contains(search));
        }

        public IQueryable<DiagnoseHistory> SearchDiagnosesHistories(int diagnoseId, string search)
        {
            return GetDiagnosesHistories(diagnoseId).Where(x => x.Conclusion.Contains(search)
                || x.Diagnose.Name == search);
        }

        public IQueryable<Examination> SearchExaminations(int cardId, string search)
        {
            return GetExaminations(cardId).Where(x => x.Type.Contains(search) || x.Result.Contains(search));
        }

        public IQueryable<PacientVaccination> SearchPacientVaccinations(int cardId, string search)
        {
            return GetPacientVaccinations(cardId).Where(x => x.Vaccination.Type.Contains(search)
                || x.Result.Contains(search));
        }

        public async Task AddAllergy(string pacientId, Allergy allergy)
        {
            allergy.Card = await _dbContext.Cards.Where(x => x.Pacient.Id == pacientId).FirstOrDefaultAsync();
            allergy.DateOfIssue = DateTime.Now;
            _dbContext.Allergies.Add(allergy);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddVaccine(string pacientId, Vaccination vaccination, DateTime date, string result)
        {
            var card = await GetCardByIdAsync(pacientId);
            var pv = new PacientVaccination
            {
                Card = card,
                Date = date,
                Result = result,
                Vaccination = vaccination,
            };
            _dbContext.PacientVaccinations.Add(pv);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Vaccination> GetVaccineById(int vaccination)
        {
            return await _dbContext.Vaccinations.Where(x => x.Id == vaccination).FirstOrDefaultAsync();
        }

        public async Task AddExamination(string pacientId, Doctor doc, Examination examination)
        {
            var card = await GetCardByIdAsync(pacientId);
            examination.Card = card;
            if (doc != null)
                examination.Doctor = doc;
            await _dbContext.Examinations.AddAsync(examination);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddDiagnose(string pacientId, Doctor doc,
            Diagnose diagnose, DiagnoseHistory history)
        {
            var card = await GetCardByIdAsync(pacientId);
            diagnose.Card = card;

            if (doc != null)
            {
                diagnose.DoctorEstablishe = doc;
                history.Doctor = doc;
            }
            diagnose.Status = false;
            await _dbContext.Diagnoses.AddAsync(diagnose);

            history.Diagnose = diagnose;
            await _dbContext.DiagnoseHistories.AddAsync(history);

            diagnose.DiagnoseHistorie.Append(history);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ConfirmDiagnose(int id, Doctor doc, DateTime confirmDate)
        {
            var diagnose = await _dbContext.Diagnoses.FirstOrDefaultAsync(x => x.Id == id);
            diagnose.Status = true;
            diagnose.DoctorConfirm = doc;
            diagnose.ConfirmDate = confirmDate;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Diagnose> GetDiagnoseById(int value, bool includePacient)
        {
            var diagnose = _dbContext.Diagnoses.Where(x => x.Id == value);
            if (includePacient)
                diagnose = diagnose.Include(x => x.Card).Include(x => x.Card.Pacient);
            return await diagnose.FirstOrDefaultAsync();
        }
        public async Task AddDiagnoseHistory(int diagnoseId, DiagnoseHistory history, Doctor doc)
        {
            var diagnose = await GetDiagnoseById(diagnoseId, true);
            history.Diagnose = diagnose;
            history.Doctor = doc;
            await _dbContext.DiagnoseHistories.AddAsync(history);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddReccomendation(string pacientId, Recommendation reccomendation, Doctor doc)
        {
            var card = await GetCardByIdAsync(pacientId);
            reccomendation.Card = card;
            reccomendation.Doctor = doc;
            await _dbContext.Recommendations.AddAsync(reccomendation);
            await _dbContext.SaveChangesAsync();
        }

    }
}
