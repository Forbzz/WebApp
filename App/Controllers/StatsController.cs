using App.ViewModels.Stats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using System;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class StatsController : Controller
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService stats)
        {
            _statsService = stats;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DiagnoseStats(string returnUrl)
        {
            var now = DateTime.Now;
            var model = new DiagnoseStatsViewModel
            {
                Start = new DateTime(now.Year, now.Month, 1),
                End = now.Date,
                ReturnUrl = returnUrl
            };

            model.Stats = await _statsService.GetIssueStats(model.Start, model.End);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DiagnoseStats(DiagnoseStatsViewModel model)
        {
            model.Stats = await _statsService.GetIssueStats(model.Start, model.End);
            return View(model);
        }

    }
}
