using Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICardService
    {
        Task<Card> GetCardByIdAsync(string userId);
        IQueryable<Allergy> GetAllergies(int cardId);
        IQueryable<Diagnose> GetDiagnoses(int cardId);
        IQueryable<DiagnoseHistory> GetDiagnoseHistories(int cardId);
        IQueryable<DiagnoseHistory> GetDiagnosesHistories(int diagnoseId);
        IQueryable<Examination> GetExaminations(int cardId);
        IQueryable<PacientVaccination> GetPacientVaccinations(int cardId);
        IQueryable<Vaccination> GetVaccinations();
        IQueryable<Allergy> SearchAllergies(int cardId, string search);
        IQueryable<Diagnose> SearchDiagnoses(int cardId, string search);
        IQueryable<DiagnoseHistory> SearchDiagnoseHistories(int cardId, string search);
        IQueryable<DiagnoseHistory> SearchDiagnosesHistories(int diagnoseId, string search);
        IQueryable<Examination> SearchExaminations(int cardId, string search);
        IQueryable<PacientVaccination> SearchPacientVaccinations(int cardId, string search);
        Task AddAllergy(string pacientId, Allergy allergy);
        Task AddVaccine(string pacientId, Vaccination vaccination, DateTime date, string result);
        Task<Vaccination> GetVaccineById(int vaccination);
        Task AddExamination(string pacientId, Doctor doc, Examination examination);
        Task AddDiagnose(string pacientId, Doctor doc, Diagnose diagnose, DiagnoseHistory history);
        Task ConfirmDiagnose(int id, Doctor doc, DateTime confirmDate);
        Task<Diagnose> GetDiagnoseById(int value, bool IncludePacient);
        Task AddDiagnoseHistory(int diagnoseId, DiagnoseHistory history, Doctor doc);
        IQueryable<Recommendation> GetReccomendations(int id);
        IQueryable<Recommendation> SearchReccomendations(int id, string search);
        Task AddReccomendation(string pacientId, Recommendation reccomendation, Doctor doc);

    }
}
