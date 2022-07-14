using AutoMapper;
using Backend.DbInfrastructure;
using Backend.Dto;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class PorudzbinaService : IPorudzbinaService
    {
        private readonly IMapper _mapper;
        private readonly DostavaAppDbContext _dbContext;

        public PorudzbinaService(IMapper mapper, DostavaAppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;

        }

        public void AzurirajPorudzbinu(long porudzbinaId, PorudzbinaDto porudzbinaDto)
        {
            var porudzbina = _dbContext.Porudzbine.Find(porudzbinaId);

            if (porudzbina == null)
            {
                return;
            }

            porudzbina = _mapper.Map<Porudzbina>(porudzbinaDto);

            _dbContext.SaveChanges();
        }

        public PorudzbinaAllDto DostavljacPoslednjaPorudzbina(long porudzbinaId)
        {
            var porudzbina = _mapper.Map<PorudzbinaAllDto>(_dbContext.Porudzbine.Include("PoruceniProizvodi.Proizvod").Where(p => p.Id == porudzbinaId).FirstOrDefault());

            if (porudzbina == null)
            {
                return null;
            }

            if (porudzbina.VremeIsporuke < DateTime.Now)
            {
                return new PorudzbinaAllDto()
                {
                    Id = -1,
                };
            }

            return porudzbina;
        }

        public PorudzbinaAllDto DostavljacPorudzbina(long porudzbinaId)
        {
            var porudzbina = _mapper.Map<PorudzbinaAllDto>(_dbContext.Porudzbine.Include("PoruceniProizvodi.Proizvod").Where(p => p.Id == porudzbinaId).FirstOrDefault());


            if (porudzbina == null)
            {
                return null;
            }

            // Jos nije isporucena
            if (porudzbina.VremeIsporuke > DateTime.Now)
            {
                return new PorudzbinaAllDto()
                {
                    Id = -1,
                };
            }

            return porudzbina;
        }

        public TrenutnaPorudzbinaDto DostavljacTrenutnaPorudzbina(long porudzbinaId)
        {
            var porudzbina = _mapper.Map<PorudzbinaAllDto>(_dbContext.Porudzbine.Include("PoruceniProizvodi.Proizvod").Where(p => p.Id == porudzbinaId).FirstOrDefault());

            if (porudzbina == null)
            {
                return null;
            }

            if (porudzbina.VremeIsporuke < DateTime.Now)
            {
                TrenutnaPorudzbinaDto retValError = new TrenutnaPorudzbinaDto()
                {
                    Porudzbina = new PorudzbinaAllDto() { Id = -1 },
                    Timer = new TimerDto() { Minutes = 0, Seconds = 0 }
                };

                return retValError;
            }

            var vremeIsporuke = porudzbina.VremeIsporuke - DateTime.Now;

            TimerDto timer = new TimerDto()
            {
                Minutes = vremeIsporuke.Minutes,
                Seconds = vremeIsporuke.Seconds
            };
            TrenutnaPorudzbinaDto retVal = new TrenutnaPorudzbinaDto()
            {
                Porudzbina = porudzbina,
                Timer = timer
            };
            return retVal;
        }

        public TrenutnaPorudzbinaDto TrenutnaPorudzbina(long userId)
        {
            var korisnici = _dbContext.Korisnici.Include("MojePorudzbine.PoruceniProizvodi.Proizvod").ToList();
            var korisnik = korisnici.Where(k => k.Id == userId).FirstOrDefault();

            if (korisnik == null)
            {
                return null;
            }

            var korisnickaTrenutnaPorudzbina = _mapper.Map<List<PorudzbinaAllDto>>(korisnik.MojePorudzbine.Where(p => p.VremeIsporuke > DateTime.Now)).FirstOrDefault();

            if (korisnickaTrenutnaPorudzbina == null)
            {
                return null;
            }


            var vremeIsporuke = korisnickaTrenutnaPorudzbina.VremeIsporuke - DateTime.Now;
            TimerDto timer = new TimerDto()
            {
                Minutes = vremeIsporuke.Minutes,
                Seconds = vremeIsporuke.Seconds
            };

            TrenutnaPorudzbinaDto retVal = new TrenutnaPorudzbinaDto()
            {
                Porudzbina = korisnickaTrenutnaPorudzbina,
                Timer = timer
            };

            return retVal;
        }

        public List<PorudzbinaAllDto> MojePorudzbine(long userId)
        {
            var korisnici = _dbContext.Korisnici.Include("MojePorudzbine.PoruceniProizvodi.Proizvod").ToList();
            var korisnik = korisnici.Where(k => k.Id == userId).FirstOrDefault();

            if (korisnik == null)
            {
                return null;
            }

            // Samo isporucene
            var korisnickeIsporucenePorudzbine = _mapper.Map<List<PorudzbinaAllDto>>(korisnik.MojePorudzbine.Where(p => p.VremeIsporuke < DateTime.Now));

            return korisnickeIsporucenePorudzbine;
        }


        public PorudzbinaDto NovaPorudzbina(long userId, PorudzbinaDto porudzbina)
        {
            var korisnik = _dbContext.Korisnici.Find(userId);

            if (korisnik == null)
            {
                return null;
            }

            if (korisnik.MojePorudzbine == null)
            {
                korisnik.MojePorudzbine = new List<Porudzbina>();
            }

            Porudzbina novaPorudzbinaSave = _mapper.Map<Porudzbina>(porudzbina);

            novaPorudzbinaSave.VremeIsporuke = DateTime.Now.AddYears(1);
            novaPorudzbinaSave.Potvrdjena = false;
            novaPorudzbinaSave.Isporucena = false;


            korisnik.MojePorudzbine.Add(novaPorudzbinaSave);
            _dbContext.SaveChanges();

            PorudzbinaDto retVal = _mapper.Map<PorudzbinaDto>(korisnik.MojePorudzbine.Last());

            return retVal;
        }

        public List<PorudzbinaAllDto> SvePorudzbine()
        {
            var svePorudzbine = _mapper.Map<List<PorudzbinaAllDto>>(_dbContext.Porudzbine.Include("PoruceniProizvodi.Proizvod"));

            svePorudzbine.ForEach(p =>
            {
                if (p.VremeIsporuke < DateTime.Now)
                {
                    p.Isporucena = true;
                }
            });

            return svePorudzbine;
        }

        public List<PorudzbinaAllDto> SveNepotvrdjenePorudzbine()
        {
            var nepotvrdjenePorudzbine = _dbContext.Porudzbine.Include("PoruceniProizvodi.Proizvod").Where(p => p.Potvrdjena == false).ToList();

            return _mapper.Map<List<PorudzbinaAllDto>>(nepotvrdjenePorudzbine);
        }

        public long PotvrdiPorudzbinu(long porudzbinaId)
        {
            var porudzbina = _dbContext.Porudzbine.Find(porudzbinaId);

            if (porudzbina == null)
            {
                return -1;
            }

            Random r = new();
            int vremeDostave = r.Next(1, 5);
            //int vremeDostave = r.Next(1, 1);
            //int vremeDostave = r.Next(10, 10);
            porudzbina.VremeIsporuke = DateTime.Now.AddMinutes(vremeDostave);
            porudzbina.Potvrdjena = true;
            _dbContext.SaveChanges();

            return porudzbina.Id;
        }

        public PorudzbinaDto NovaPorudzbina(KorisnikDto user, PorudzbinaDto porudzbina)
        {
            throw new NotImplementedException();
        }

        
    }
}
