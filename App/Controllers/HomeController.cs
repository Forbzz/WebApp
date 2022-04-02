using App.Models;
using App.ViewModels.Home;
using Data.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IDoctorService doctorService,
            IScheduleService scheduleService, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Specialities()
        {
            var vm = new SpecialitiesViewModel
            {
                Specialities = await _doctorService.GetSpecialities().Select(x => x).ToListAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> DoctorList(int specId)
        {
            var docs = await _doctorService.GetDoctorsBySpec(specId).ToArrayAsync();

            return View(new HomeDoctorsViewModel { Doctors = docs });
        }

        public async Task<IActionResult> GetEvents(string id, DateTime start, DateTime end)
        {
            var events = await _scheduleService.GetFreeTicketsCount(id, start, end);
            var json = Json(events);
            return json;
        }

        public async Task<IActionResult> Schedule(string Id)
        {
            ViewBag.ID = Id;
            return View();
        }

        public async Task<IActionResult> Tickets(string Id, DateTime date)
        {
            var tickets = (await _scheduleService.GetFreeTickets(Id, date))
                .Select(x => new { Time = x.Time.ToString(@"hh\:mm"), id = x.Id });
            return Json(tickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
