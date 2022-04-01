using Data.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.ViewModels.Home
{
    public class TicketsViewModel
    {
        public string userId { get; set; }

        public string ReturnUrl { get; set; }

        public string Header { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public bool IsError { get; set; }

        public StatusViewModel StatusModel => new StatusViewModel()
        {
            StatusMessage = this.StatusMessage,
            IsError = this.IsError
        };
    }
}
