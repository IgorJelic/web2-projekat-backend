using AutoMapper;
using Backend.DbInfrastructure;
using Backend.Dto;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ProizvodService : IProizvodService
    {
        private readonly IMapper _mapper;
        private readonly DostavaAppDbContext _dbContext;

        public ProizvodService(IMapper mapper, DostavaAppDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "administrator")]
        public ProizvodDto AddNewProizvod(ProizvodDto noviProizvod)
        {
            var proizvod = _dbContext.Proizvodi.FirstOrDefault(p => p.Ime == noviProizvod.Ime);

            if (proizvod != null)
            {
                return null;
            }

            _dbContext.Proizvodi.Add(_mapper.Map<Proizvod>(noviProizvod));
            _dbContext.SaveChanges();

            return noviProizvod;
        }

        public List<ProizvodDto> GetAllProizvodi()
        {
            return _mapper.Map<List<ProizvodDto>>(_dbContext.Proizvodi);
        }

        public ProizvodDto GetProizvod(long id)
        {
            var proizvod = _dbContext.Proizvodi.Find(id);

            if (proizvod == null)
            {
                return null;
            }

            return _mapper.Map<ProizvodDto>(proizvod);
        }

        public void RemoveProizvod(long id)
        {
            var proizvod = _dbContext.Proizvodi.Find(id);

            if (proizvod != null)
            {
                _dbContext.Proizvodi.Remove(proizvod);
                _dbContext.SaveChanges();
            }
        }
    }
}
