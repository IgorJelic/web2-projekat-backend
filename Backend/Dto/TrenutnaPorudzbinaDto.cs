using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto
{
    public class TrenutnaPorudzbinaDto
    {
        public TimerDto Timer { get; set; }
        public PorudzbinaAllDto Porudzbina { get; set; }    
    }
}
