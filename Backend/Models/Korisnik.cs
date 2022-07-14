using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Korisnik
    {
        public long Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string ProfilnaSlikaPath { get; set; }
        public string TipKorisnika { get; set; }
        public bool Aktiviran { get; set; }
        public List<Porudzbina> MojePorudzbine { get; set; }

        // Preuzeta porudzbina, ako je korisnik dostavljac
    }
}
