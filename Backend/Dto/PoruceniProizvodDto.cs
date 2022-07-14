using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class PoruceniProizvodDto
    {
        public long Id { get; set; }
        public double Kolicina { get; set; }
        //public long ProizvodId { get; set; }
        public ProizvodDto Proizvod { get; set; }

    }
}
