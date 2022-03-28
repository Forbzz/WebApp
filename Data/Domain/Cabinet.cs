using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain
{
    public class Cabinet
    {
        [Key]
        public int Id { get; set; }

        public int? Number { get; set; }

        public string Name { get; set; }

    }
}
