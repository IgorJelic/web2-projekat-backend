using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class KorisnikRegisterDto
    {
        public long Id { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
        [Required]

        public DateTime DatumRodjenja { get; set; }
        [Required]

        public string ProfilnaSlikaPath { get; set; }
        [Required]

        public string TipKorisnika { get; set; }
    }
}
