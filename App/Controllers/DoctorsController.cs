using App.ViewModels.Doctors;
using Data.Domain;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Repository.Interface;
using App.ViewModels.User;
using App.ViewModels.Stats;

namespace App.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDoctorService _doctorService;
        private readonly IStatsService _statsService;
        private readonly UserManager<AppUser> _userManager;

        public DoctorsController(ILogger<UsersController> logger, IDoctorService doctorService, IStatsService statsService, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _doctorService = doctorService;
            _statsService = statsService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DoctorSearchViewModel();
            model.Branches = await _doctorService.GetBranches().Select(x => x.Name).ToListAsync();
            model.Specialities = await _doctorService.GetSpecialities().Select(x => x.Name).ToListAsync();
            model.Doctors = await _doctorService.GetDoctors().ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DoctorSearchViewModel model)
        {
            model.Branches = await _doctorService.GetBranches().Select(x => x.Name).ToListAsync();
            model.Specialities = await _doctorService.GetSpecialities().Select(x => x.Name).ToListAsync();
            model.Doctors = await _doctorService.GetDoctors(model.Name, model.Speciality, model.Branch).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(string id, string returnUrl)
        {
            var doc = await _doctorService.GetDoctorByIdAsync(id);
            var model = new DetailsDoctorViewModel
            {
                Doctor = doc,
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            var model = new DoctorEditViewModel();
            model.ReturnUrl = returnUrl;
            var doc = await _doctorService.GetDoctorByIdAsync(id);
            var specs = await _doctorService.GetSpecialities()
                    .Select(x => x.Name).ToListAsync();
            var branches = await _doctorService.GetBranches()
                .Select(x => x.Name).ToListAsync();

            model.EditModel = new EditUserViewModel(doc, "Doctor", branches, specs);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var docModel = model.EditModel;
            await _doctorService.UpdateAsync(docModel.Id, docModel.Name1, docModel.Name2,
                    docModel.Name3, docModel.Branch, docModel.Speciality, docModel.ContactEmail,
                    docModel.ContactPhoneNumber, docModel.CabinetNumber);

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return LocalRedirect(model.ReturnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Stats(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
                id = _userManager.GetUserId(User);
            var now = DateTime.Now;

            var model = new DoctorStatsViewModel()
            {
                Start = new DateTime(now.Year, now.Month, 1),
                End = now.Date,
                ReturnUrl = returnUrl
            };

            model.Stats = await _statsService.GetDoctorStats(id, model.Start, model.End);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Stats(DoctorStatsViewModel model)
        {
            model.Stats = await _statsService.GetDoctorStats(model.Id, model.Start, model.End);
            return View(model);
        }


    }
}
