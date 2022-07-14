using Backend.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IUserService
    {
        TokenDto Login(KorisnikLoginDto user);
        KorisnikDto Register(KorisnikRegisterDto user);
        List<KorisnikDto> GetAllUsers();
        List<DostavljacDto> GetAllUnActivatedUsers();
        List<DostavljacDto> GetAllDostavljaci();
        KorisnikRegisterDto GetUser(long id);
        KorisnikRegisterDto UpdateUser(long id, KorisnikRegisterDto izmenjeniKorisnik);
        KorisnikDto ActivateUser(long id);
        string UpdateProfilePhoto(long id, IFormFile file);
        string UploadFile(IFormFile file);
    }
}
