using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public long KorisnikId { get; set; }
        public bool Aktiviran { get; set; }
    }
}
