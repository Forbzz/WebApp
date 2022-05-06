using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public Pacient Pacient { get; set; }
        public Schedule Schedule { get; set; }
        public DateTime TicketDate { get; set; }
        public bool Status { get; set; }
        public bool Confirmed { get; set; } // подтверждение админами талона на прием
    }
}
