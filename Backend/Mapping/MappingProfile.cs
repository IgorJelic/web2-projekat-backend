using AutoMapper;
using Backend.Dto;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Korisnik, KorisnikLoginDto>().ReverseMap();
            CreateMap<Korisnik, KorisnikRegisterDto>().ReverseMap();
            CreateMap<Korisnik, KorisnikDto>().ReverseMap();
            CreateMap<Korisnik, DostavljacDto>().ReverseMap();
            CreateMap<Proizvod, ProizvodDto>().ReverseMap();
            CreateMap<Porudzbina, PorudzbinaDto>().ReverseMap();
            CreateMap<Porudzbina, PorudzbinaAllDto>().ReverseMap();
            CreateMap<PoruceniProizvod, PoruceniProizvodDto>().ReverseMap();
        }
    }
}
