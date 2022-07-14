using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Proizvod
    {
        public long Id { get; set; }
        public string Ime { get; set; }

        [Range(0, double.MaxValue)]
        public double Cena { get; set; }
        public string Sastojci { get; set; }

        public List<PoruceniProizvod> PoruceniProizvodi { get; set; }
    }
}
