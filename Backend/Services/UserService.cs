using AutoMapper;
using Backend.DbInfrastructure;
using Backend.Dto;
using Backend.Models;
using Backend.Services.Interfaces;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DostavaAppDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly IConfigurationSection _secretKey;


        public UserService(IConfiguration configuration, IMapper mapper, DostavaAppDbContext dbContext, IEmailSender emailSender)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _emailSender = emailSender;
            _secretKey = configuration.GetSection("SecretKey");
        }

        public KorisnikDto ActivateUser(long id)
        {
            var korisnik = _dbContext.Korisnici.Find(id);

            if (korisnik == null)
            {
                return null;
            }
            korisnik.Aktiviran = true;
            _dbContext.SaveChanges();

            var emailAddress = korisnik.Email;
            var message = new Message(new string[] { $"{emailAddress}" }, "Aktivacija profila", "Vas profil je aktiviran! DostavaApp by Igor Jelic");
            _emailSender.SendEmail(message);

            return _mapper.Map<KorisnikDto>(korisnik);
        }

        public List<DostavljacDto> GetAllUnActivatedUsers()
        {
            List<DostavljacDto> retVal = _mapper.Map<List<DostavljacDto>>(_dbContext.Korisnici.Where(k => k.Aktiviran == false));

            return retVal;
        }

        [Authorize(Roles = "administrator")]
        public List<DostavljacDto> GetAllDostavljaci()
        {
            List<DostavljacDto> retVal = _mapper.Map<List<DostavljacDto>>(_dbContext.Korisnici.Where(k => k.TipKorisnika == "dostavljac"));

            return retVal;
        }

        public List<KorisnikDto> GetAllUsers()
        {
            return _mapper.Map<List<KorisnikDto>>(_dbContext.Korisnici);
        }

        public TokenDto Login(KorisnikLoginDto user)
        {
            //Korisnik korisnik = _dbContext.Korisnici.FirstOrDefault(k => k.Email == user.Email && k.Aktiviran);
            Korisnik korisnik = _dbContext.Korisnici.FirstOrDefault(k => k.Email == user.Email);

            if (user == null || korisnik == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(user.Password, korisnik.Password))
            {
                List<Claim> userClaims = new List<Claim>();

                if (korisnik.TipKorisnika == "administrator")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "administrator"));
                }
                if (korisnik.TipKorisnika == "dostavljac")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "dostavljac"));
                }
                if (korisnik.TipKorisnika == "potrosac")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "potrosac"));
                }

                //userClaims.Add(new Claim("LoggedInClaim", "LoggedIn"));

                //Kreiramo kredencijale za potpisivanje tokena. Token mora biti potpisan privatnim kljucem
                //kako bi se sprecile njegove neovlascene izmene
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    //issuer: "https://localhost:44370/", //url servera koji je izdao token
                    issuer: "https://localhost:44370/", //url servera koji je izdao token
                    claims: userClaims, //claimovi
                    expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                    signingCredentials: signinCredentials //kredencijali za potpis
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return new TokenDto()
                {
                    Token = tokenString,
                    Role = korisnik.TipKorisnika,
                    KorisnikId = korisnik.Id,
                    Aktiviran = korisnik.Aktiviran
                };
            }
            else
            {
                return null;
            }
        }


        public string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return fileName;
                }
            }

            return "";
        }

        public KorisnikDto Register(KorisnikRegisterDto user)
        {
            Korisnik korisnik = _dbContext.Korisnici.FirstOrDefault(k => k.Email == user.Email);

            if (korisnik != null)
            {
                return null;
            }

            Korisnik noviKorisnik = _mapper.Map<Korisnik>(user);

            noviKorisnik.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            noviKorisnik.MojePorudzbine = new List<Porudzbina>();

            if (noviKorisnik.TipKorisnika == "dostavljac")
            {
                noviKorisnik.Aktiviran = false;
            }
            else
            {
                noviKorisnik.Aktiviran = true;
            }
      

            //noviKorisnik.ProfilnaSlikaPath = "";

            _dbContext.Korisnici.Add(noviKorisnik);
            _dbContext.SaveChanges();

            return _mapper.Map<KorisnikDto>(noviKorisnik);
        }

        public KorisnikRegisterDto GetUser(long id)
        {
            var korisnik = _dbContext.Korisnici.Find(id);

            if (korisnik == null)
            {
                return null;
            }

            return _mapper.Map<KorisnikRegisterDto>(korisnik);
        }

        public KorisnikRegisterDto UpdateUser(long id, KorisnikRegisterDto izmenjeniKorisnik)
        {
            var korisnik = _dbContext.Korisnici.Find(id);

            if (korisnik == null)
            {
                return null;
            }
            else
            {
                korisnik.Ime = izmenjeniKorisnik.Ime;
                korisnik.Prezime = izmenjeniKorisnik.Prezime;
                korisnik.Email = izmenjeniKorisnik.Email;
                korisnik.DatumRodjenja = izmenjeniKorisnik.DatumRodjenja;

                _dbContext.SaveChanges();
                return _mapper.Map<KorisnikRegisterDto>(korisnik);
            }
        }

        public string UpdateProfilePhoto(long id, IFormFile file)
        {
            var korisnik = _dbContext.Korisnici.Find(id);

            if (korisnik == null)
            {
                return "";
            }
            else
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var folderName = Path.Combine("Resources", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        korisnik.ProfilnaSlikaPath = fileName;
                        _dbContext.SaveChanges();

                        return fileName;
                    }
                }

                return "";

                //var folderName = Path.Combine("Resources", "Images");
                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //var fullPath = Path.Combine(pathToSave, fileName);

                //using (var stream = new FileStream(fullPath, FileMode.Create))
                //{
                //    file.CopyTo(stream);
                //}

                //korisnik.ProfilnaSlikaPath = fileName;
                //_dbContext.SaveChanges();

                //return fileName;
            }
        }
    }
}
