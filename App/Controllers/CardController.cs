using App.ViewModels.Cards;
using Data.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class CardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICardService _cardService;
        private readonly IUserService _userService;
        private readonly IPacientService _pacientService;
        private readonly IServicesService _servicesService;
        public CardController(UserManager<AppUser> userManager, ICardService cardService,
            IUserService userService, IPacientService pacientService, IServicesService servicesService)
        {
            _userManager = userManager;
            _cardService = cardService;
            _userService = userService;
            _pacientService = pacientService;
            _servicesService = servicesService;
        }

        public async Task<IActionResult> Index(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = _userManager.GetUserId(User);
            }

            var pacient = await _pacientService.GetPacientByIdAsync(id, true, true, false);
            var card = await _cardService.GetCardByIdAsync(id);
            var model = new CardViewModel
            {
                Pacient = pacient,
                Card = card,
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        public async Task<IActionResult> Search(string id, string returnUrl, string search)
        {
            var pacient = await _pacientService.GetPacientByIdAsync(id, false, false, false);
            var card = await _cardService.GetCardByIdAsync(id);
            IQueryable<Allergy> allergies;
            IQueryable<Examination> examinations;
            IQueryable<PacientVaccination> pacientVaccinations;
            IQueryable<Diagnose> diagnoses;
            IQueryable<DiagnoseHistory> diagnoseHistories;
            IQueryable<Recommendation> recommendations;

            if (string.IsNullOrEmpty(search))
            {
                allergies = _cardService.GetAllergies(card.Id);
                examinations = _cardService.GetExaminations(card.Id);
                pacientVaccinations = _cardService.GetPacientVaccinations(card.Id);
                diagnoses = _cardService.GetDiagnoses(card.Id);
                diagnoseHistories = _cardService.GetDiagnosesHistories(card.Id);
                recommendations = _cardService.GetReccomendations(card.Id);
            }
            else
            {
                allergies = _cardService.SearchAllergies(card.Id, search);
                examinations = _cardService.SearchExaminations(card.Id, search);
                pacientVaccinations = _cardService.SearchPacientVaccinations(card.Id, search);
                diagnoses = _cardService.SearchDiagnoses(card.Id, search);
                diagnoseHistories = _cardService.SearchDiagnoseHistories(card.Id, search);
                recommendations = _cardService.SearchReccomendations(card.Id, search);

            }

            card.Allergies = await allergies.OrderBy(x => x.DateOfIssue)
                .Take(10).ToListAsync();
            card.Examinations = await examinations.OrderBy(x => x.ExaminationDate)
                .Take(10).ToListAsync();
            card.Vaccinations = await pacientVaccinations.OrderBy(x => x.Date)
                .Include(x => x.Vaccination).Take(10).ToListAsync();
            card.Diagnoses = await diagnoses.OrderBy(x => x.EstablisheDate).Take(10)
                .Include(x => x.DoctorEstablishe)
                .Include(x => x.DoctorConfirm)
                .ToListAsync();
            card.Recommendations = await recommendations.OrderBy(x => x.Start)
                .Take(10).ToListAsync();

            var allergyCount = await allergies.CountAsync();
            var diagnoseHistoryCount = await diagnoseHistories.CountAsync();
            var diagnoseCount = await diagnoses.CountAsync();
            var examinationsCount = await examinations.CountAsync();
            var vaccinationsCount = await pacientVaccinations.CountAsync();
            var recommendationsCount = await recommendations.CountAsync();

            var model = new CardSearchViewModel
            {
                Search = search,
                Pacient = pacient,
                Card = card,
                ReturnUrl = returnUrl,
                AllrgeysCount = allergyCount,
                DiagnoseHistoryCount = diagnoseHistoryCount,
                DiagnoseHistories = diagnoseHistories.OrderBy(x => x.ConclusionDate).Take(10),
                DiagnosesCount = diagnoseCount,
                ExaminationsCount = examinationsCount,
                VaccinationsCount = vaccinationsCount,
                ReccomendationsCount = recommendationsCount
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(CardSearchViewModel model)
        {
            return RedirectToAction(nameof(Search), new
            {
                id = model.Pacient.Id,
                returnUrl = model.ReturnUrl,
                search = model.Search
            });
        }

        public async Task<IActionResult> Allergies(string id, string returnUrl, string search)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            var model = new AllergiesViewModel
            {
                PacientId = id,
                returnUrl = returnUrl,
                CardId = card.Id,
                Search = search
            };
            if (!string.IsNullOrEmpty(search))
            {
                model.Allergies = await _cardService.SearchAllergies(card.Id, search)
                    .OrderBy(x => x.DateOfIssue).ToListAsync();
            }
            else
            {
                model.Allergies = await _cardService.GetAllergies(card.Id)
                    .OrderBy(x => x.DateOfIssue).ToListAsync();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Allergies(AllergiesViewModel model)
        {
            return RedirectToAction(nameof(Allergies), new
            {
                Id = model.PacientId,
                returnUrl = model.returnUrl,
                search = model.Search
            });
        }

        public IActionResult AddAllergy(string Id, int cardId, string returnUrl)
        {
            return View(new AddAlergyViewModel
            {
                CardId = cardId,
                PacientId = Id,
                ReturnUrl = returnUrl,
                Allergy = new Allergy()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddAllergy(AddAlergyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _cardService.AddAllergy(model.PacientId, model.Allergy);

            return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> Vaccinations(string Id, string returnUrl, string search)
        {
            var model = new VaccinationsViewModel
            {
                PacientId = Id,
                ReturnUrl = returnUrl,
                Search = search
            };
            var card = await _cardService.GetCardByIdAsync(Id);
            if (!string.IsNullOrEmpty(search))
            {
                model.Vaccinations = await _cardService.SearchPacientVaccinations(card.Id, search)
                    .OrderBy(x => x.Date).Include(x => x.Vaccination).ToListAsync();
            }
            else
            {
                model.Vaccinations = await _cardService.GetPacientVaccinations(card.Id)
                    .OrderBy(x => x.Date).Include(x => x.Vaccination).ToListAsync();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Vaccinations(VaccinationsViewModel viewModel)
        {
            return RedirectToAction(nameof(Vaccinations), new
            {
                Id = viewModel.PacientId,
                returnUrl = viewModel.ReturnUrl,
                search = viewModel.Search
            });
        }
        public async Task<IActionResult> AddVaccine(string Id, string returnUrl)
        {
            var vaccinations = await _servicesService.GetVaccinations(false).ToArrayAsync();
            var model = new AddVaccinationViewModel
            {
                PacientId = Id,
                ReturnUrl = returnUrl,
                Vaccinations = vaccinations,
                Date = DateTime.Now.Date
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddVaccine(AddVaccinationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var vaccine = await _cardService.GetVaccineById(model.Vaccination);
            await _cardService.AddVaccine(model.PacientId, vaccine,
                model.Date, model.Result);

            return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> Examinations(string id, string returnUrl, string search)
        {
            var model = new ExaminationViewModel
            {
                PacientId = id,
                ReturnUrl = returnUrl,
                Search = search
            };

            var card = await _cardService.GetCardByIdAsync(id);
            if (!string.IsNullOrEmpty(search))
            {
                model.Examinations = await _cardService.SearchExaminations(card.Id, search)
                    .OrderBy(x => x.ExaminationDate).ToListAsync();
            }

            else
            {
                model.Examinations = await _cardService.GetExaminations(card.Id).Include(x=>x.Doctor)
                    .OrderBy(x => x.ExaminationDate).ToListAsync();
            }

            
            return View(model);
        }

        [HttpPost]
        public IActionResult Examinations(ExaminationViewModel model)
        {
            return RedirectToAction(nameof(Examinations), new
            {
                Id = model.PacientId,
                returnUrl = model.ReturnUrl,
                search = model.Search
            });
        }

        public IActionResult AddExamination(string Id, string returnUrl)
        {
            var model = new AddExaminationViewModel
            {
                PacientId = Id,
                ReturnUrl = returnUrl,
                Examination = new Examination()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddExamination(AddExaminationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Doctor doc = null;
            if (User.IsInRole("Doctor"))
            {
                doc = await _userManager.GetUserAsync(User) as Doctor;
            }
            model.Examination.ExaminationDate = DateTime.Now;
            await _cardService.AddExamination(model.PacientId, doc, model.Examination);

            return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> Diagnoses(string Id, string returnUrl, string search)
        {
            var model = new DiagnosesViewModel
            {
                PacientId = Id,
                ReturnUrl = returnUrl,
                Search = search
            };
            var card = await _cardService.GetCardByIdAsync(Id);
            if (!string.IsNullOrEmpty(search))
            {
                model.Diagnoses = await _cardService.SearchDiagnoses(card.Id, search)
                    .OrderBy(x => x.EstablisheDate).Include(x => x.DoctorEstablishe)
                    .Include(x => x.DoctorConfirm).ToListAsync();
            }
            else
            {
                model.Diagnoses = await _cardService.GetDiagnoses(card.Id)
                    .OrderBy(x => x.EstablisheDate).Include(x => x.DoctorEstablishe)
                    .Include(x => x.DoctorConfirm).ToListAsync();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Diagnoses(DiagnosesViewModel model)
        {
            return RedirectToAction(nameof(Diagnoses), new
            {
                Id = model.PacientId,
                returnUrl = model.ReturnUrl,
                search = model.Search
            });
        }

        public IActionResult AddDiagnose(string Id, string returnUrl)
        {
            var model = new AddDiagnoseViewModel
            {
                PacientId = Id,
                Diagnose = new Diagnose(),
                ReturnUrl = returnUrl,
                History = new DiagnoseHistory()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiagnose(AddDiagnoseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Doctor doc = null;

            if (User.IsInRole("Doctor"))
            {
                doc = await _userManager.GetUserAsync(User) as Doctor;
            }
            model.Diagnose.EstablisheDate = DateTime.Now;
            model.History.ConclusionDate = DateTime.Now;
            await _cardService.AddDiagnose(model.PacientId, doc, model.Diagnose, model.History);

            return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> ConfirmDiagnose(int id, string returnUrl)
        {
            Doctor doc = null;
            if (User.IsInRole("Doctor"))
            {
                doc = await _userManager.GetUserAsync(User) as Doctor;
            }
            var confirmDate = DateTime.Now;
            await _cardService.ConfirmDiagnose(id, doc, confirmDate);
            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> DiagnoseHistory(string Id, int? diagnoseId,
            string returnUrl, string search)
        {
            var card = await _cardService.GetCardByIdAsync(Id);
            var model = new DiagnoseHistoryViewModel
            {
                DiagnoseId = diagnoseId,
                PacientId = Id,
                ReturnUrl = returnUrl,
                Search = search,
            };
            if (diagnoseId.HasValue)
            {
                model.Diagnose = await _cardService.GetDiagnoseById(diagnoseId.Value, true);

                if (!string.IsNullOrEmpty(search))
                {
                    model.History = await _cardService.SearchDiagnosesHistories(diagnoseId.Value, search)
                        .OrderBy(x => x.ConclusionDate).Include(x => x.Doctor).ToArrayAsync();
                }
                else
                {
                    model.History = await _cardService.GetDiagnosesHistories(diagnoseId.Value)
                        .OrderBy(x => x.ConclusionDate).Include(x => x.Doctor).ToArrayAsync();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(search))
                {
                    model.History = await _cardService.SearchDiagnoseHistories(card.Id, search)
                        .OrderBy(x => x.ConclusionDate).Include(x => x.Diagnose).ToArrayAsync();
                }
                model.History = await _cardService.GetDiagnoseHistories(card.Id)
                    .OrderBy(x => x.ConclusionDate).Include(x => x.Diagnose).ToArrayAsync();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult DiagnoseHistory(DiagnoseHistoryViewModel model)
        {
            return RedirectToAction(nameof(DiagnoseHistory), new
            {
                Id = model.PacientId,
                diagnoseId = model.DiagnoseId,
                returnUrl = model.ReturnUrl,
                search = model.Search
            });
        }

        public IActionResult AddDiagnoseHistory(int diagnoseId, string returnUrl)
        {
            var model = new AddDiagnoseHistoryViewModel
            {
                DiagnoseId = diagnoseId,
                History = new DiagnoseHistory(),
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiagnoseHistory(AddDiagnoseHistoryViewModel model)
        {
            Doctor doc = null;
            if (User.IsInRole("Doctor"))
            {
                doc = await _userManager.GetUserAsync(User) as Doctor;
            }
            model.History.ConclusionDate = DateTime.Now;
            await _cardService.AddDiagnoseHistory(model.DiagnoseId, model.History, doc);
            return LocalRedirect(model.ReturnUrl);
        }

        public async Task<IActionResult> Recommendations(string Id, string returnUrl, string search)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = _userManager.GetUserId(User);
            }
            var model = new RecommendationViewModel
            {
                PacientId = Id,
                ReturnUrl = returnUrl,
                Search = search
            };
            var card = await _cardService.GetCardByIdAsync(Id);
            if (!string.IsNullOrEmpty(search))
            {
                model.Reccomendations = await _cardService.SearchReccomendations(card.Id, search)
                    .OrderBy(x => x.Start).ToListAsync();
            }
            else
            {
                model.Reccomendations = await _cardService.GetReccomendations(card.Id)
                    .OrderBy(x => x.Start).ToListAsync();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Recommendations(RecommendationViewModel model)
        {
            return RedirectToAction(nameof(Recommendations), new
            {
                Id = model.PacientId,
                returnUrl = model.ReturnUrl,
                search = model.Search
            });
        }

        public IActionResult AddRecommendation(string Id, string returnUrl)
        {
            var model = new AddRecommendationViewModel
            {
                PacientId = Id,
                Recommendation = new Recommendation(),
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecommendation(AddRecommendationViewModel model)
        {
            Doctor doc = null;
            if (User.IsInRole("Doctor"))
            {
                doc = await _userManager.GetUserAsync(User) as Doctor;
            }
            model.Recommendation.Start = DateTime.Now;
            await _cardService.AddReccomendation(model.PacientId, model.Recommendation, doc);
            return LocalRedirect(model.ReturnUrl);
        }
    }
}
