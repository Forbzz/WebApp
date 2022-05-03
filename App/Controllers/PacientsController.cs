using App.ViewModels;
using App.ViewModels.Pacients;
using Data.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class PacientsController : Controller
    {
        private readonly IPacientService _pacientService;
        private readonly UserManager<AppUser> _userManager;

        public PacientsController(IPacientService pacientService,
            UserManager<AppUser> userManager)
        {
            _pacientService = pacientService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? page)
        {
            page ??= 1;
            var vm = new PacientIndexViewModel();
            var count = await _pacientService.GetPacients().CountAsync();
            vm.PageViewModel = new PageViewModel(page.Value, 10, count);
            var skip = (page.Value - 1) * vm.PageViewModel.PageSize;
            if (skip > count)
            {
                return NotFound();
            }
            vm.PageViewModel = new PageViewModel(page.Value, 10, count);

            vm.Pacients = _pacientService
                .GetPacients(skip, vm.PageViewModel.PageSize, includeCard: true, includeAddress: true);
            vm.Search = new PacientSearchViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int? page, PacientSearchViewModel Search)
        {
            page ??= 1;
            var vm = new PacientIndexViewModel { Search = Search };

            var query = _pacientService.SearchPacient(Search.Name,
                Search.CardNumber, Search.Address);

            var count = await query.CountAsync();
            var pvm = new PageViewModel(page.Value, 10, count);

            var skip = (page.Value - 1) * pvm.PageSize;
            query = _pacientService.SearchPacient(Search.Name,
                Search.CardNumber, Search.Address, skip, pvm.PageSize, includeCard: true,
                includeAddress: true, includeContacts: true);
            if (skip > count)
            {
                return NotFound();
            }

            return View("Index", new PacientIndexViewModel
            {
                Pacients = await query.ToListAsync(),
                PageViewModel = pvm,
                Search = Search
            });
        }

        public async Task<IActionResult> Details(string id, string returnUrl)
        {
            var pacient = await _pacientService.GetPacientByIdAsync(id, true, true, false);
            pacient.Address = await _pacientService.GetPacientAddress(id);

            var model = new PacientInfoViewModel
            {
                Pacient = pacient,
                returnUrl = returnUrl
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            var model = new PacientEditViewModel
            {
                Pacient = await _pacientService.GetPacientByIdAsync(id, true, false, true),
                ReturnUrl = returnUrl
            };

            if(model.Pacient.BirthDay == null)
            {
                model.Pacient.BirthDay = DateTime.Now.Date;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PacientEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var res = await _pacientService.UpdateAsync(model.Pacient);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            return LocalRedirect(model.ReturnUrl);
        }

    }
}
