using AutoMapper;
using Backend.Dto;
using Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Backend.Models
{
    public class Porudzbina
    {
        public long Id { get; set; }
        public string AdresaDostave { get; set; }
        public string Komentar { get; set; }
        // Automatski se racuna na osnovu liste porucenih proizvoda
        public double Cena { get; set; }
        public bool Potvrdjena { get; set; }
        public bool Isporucena { get; set; }
        // Odbrojavanje do VremenaIsporuke (random izmedju 5 i 25 minuta)
        // DateTime.Now.AddMinutes(random)
        public DateTime VremeIsporuke { get; set; }

        public long KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public List<PoruceniProizvod> PoruceniProizvodi { get; set; }

        // Informacija koji dostavljac je preuzeo porudzbinu

        
    }
}
