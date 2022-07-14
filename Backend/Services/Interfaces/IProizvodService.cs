using Backend.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IProizvodService
    {
        List<ProizvodDto> GetAllProizvodi();
        ProizvodDto GetProizvod(long id);
        ProizvodDto AddNewProizvod(ProizvodDto noviProizvod);
        void RemoveProizvod(long id);
    }
}
