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
using Data;

namespace App.Controllers
{
    public class TicketController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDoctorService _doctorService;
        private readonly IStatsService _statsService;
        private readonly IPacientService _acientService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TicketController(ILogger<UsersController> logger, IDoctorService doctorService, IStatsService statsService, UserManager<AppUser> userManager, IPacientService acientService, ApplicationDbContext context)
        {
            _logger = logger;
            _doctorService = doctorService;
            _statsService = statsService;
            _userManager = userManager;
            _acientService = acientService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pacients = await _context.Tickets.Include(x => x.Pacient).Include(x => x.Schedule).ThenInclude(x=>x.Doctor)
                .Where(x => x.Confirmed == false)
                .OrderBy(x => x.TicketDate)
                .ThenBy(x => x.Schedule.Time)
                .ToListAsync();

            return View(pacients);
        }

        public async Task<IActionResult> DeclineTicket(string Id)
        {
            var ticket = await _context.Tickets.FindAsync(int.Parse(Id));
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            else
            {

                return Json(new
                {
                    status = "Error",
                    errors = ticket,
                    message = "Can't decline ticket"
                });

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApproveTicket(string Id)
        {
            var ticket = await _context.Tickets.FindAsync(int.Parse(Id));
            if (ticket != null)
            {
                ticket.Confirmed = true;
                _context.Tickets.Update(ticket);
            }
            else
            {

                return Json(new
                {
                    status = "Error",
                    errors = ticket,
                    message = "Can't approve ticket"
                });

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
