using Backend.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IPorudzbinaService
    {
        PorudzbinaDto NovaPorudzbina(long userId, PorudzbinaDto porudzbina);
        PorudzbinaDto NovaPorudzbina(KorisnikDto user, PorudzbinaDto porudzbina);
        List<PorudzbinaAllDto> SvePorudzbine();
        List<PorudzbinaAllDto> SveNepotvrdjenePorudzbine();
        long PotvrdiPorudzbinu(long porudzbinaId);
        List<PorudzbinaAllDto> MojePorudzbine(long userId);
        TrenutnaPorudzbinaDto TrenutnaPorudzbina(long userId);
        TrenutnaPorudzbinaDto DostavljacTrenutnaPorudzbina(long porudzbinaId);
        PorudzbinaAllDto DostavljacPorudzbina(long porudzbinaId);
        PorudzbinaAllDto DostavljacPoslednjaPorudzbina(long porudzbinaId);
        void AzurirajPorudzbinu(long porudzbinaId, PorudzbinaDto porudzbinaDto);
    }
}
