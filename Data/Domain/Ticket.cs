using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
