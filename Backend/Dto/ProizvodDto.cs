using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class ProizvodDto
    {
        public long Id { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public double Cena { get; set; }
        [Required]
        public string Sastojci { get; set; }
    }
}
