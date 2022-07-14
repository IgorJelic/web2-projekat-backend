using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class PoruceniProizvod
    {
        public long Id { get; set; }

        [Range(1, double.MaxValue)]
        public double Kolicina { get; set; }

        // Proizvod koji se narucuje
        public long ProizvodId { get; set; }
        public Proizvod Proizvod { get; set; }
        // Porudzbina kojoj pripada poruceni proizvod
        public long PorudzbinaId { get; set; }
        public Porudzbina Porudzbina { get; set; }
    }
}
