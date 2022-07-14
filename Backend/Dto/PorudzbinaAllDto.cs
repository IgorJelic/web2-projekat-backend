using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class PorudzbinaAllDto
    {
        public long Id { get; set; }
        public string AdresaDostave { get; set; }
        public string Komentar { get; set; }
        public double Cena { get; set; }
        public bool Potvrdjena { get; set; }
        public bool Isporucena { get; set; }
        public DateTime VremeIsporuke { get; set; }

        public List<PoruceniProizvodDto> PoruceniProizvodi { get; set; }
    }
}
