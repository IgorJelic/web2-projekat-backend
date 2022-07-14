using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class PorudzbinaDto
    {
        public long Id { get; set; }
        [Required]
        public string AdresaDostave { get; set; }
        [Required]

        public string Komentar { get; set; }
        [Required]

        public double Cena { get; set; }
        //public bool Potvrdjena { get; set; }
        //public bool Isporucena { get; set; }
        //public DateTime VremeIsporuke { get; set; }
        [Required]


        public List<PoruceniProizvodDto> PoruceniProizvodi { get; set; }

    }
}
