using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Номер карты")]
        public int? Number { get; set; }

        [Display(Name = "Дата выдачи")]
        public DateTime DateOfIssue { get; set; }

        public string PacientId { get; set; }

        [ForeignKey(nameof(PacientId))]
        public Pacient Pacient { get; set; }

        [Display(Name = "Вакцинации")]
        public ICollection<PacientVaccination> Vaccinations { get; set; }

        [Display(Name = "Осмотры")]
        public ICollection<Examination> Examinations { get; set; }

        [Display(Name = "Аллергии")]
        public ICollection<Allergy> Allergies { get; set; }

        [Display(Name = "Талоны")]
        public ICollection<Ticket> Tickets { get; set; }

        [Display(Name = "Диагнозы")]
        public IEnumerable<Diagnose> Diagnoses { get; set; }

        public IEnumerable<Recommendation> Recommendations { get; set; }


    }
}
