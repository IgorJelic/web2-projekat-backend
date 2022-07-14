using Backend.Dto;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("upload")]
        public ActionResult UploadFile()
        {
            var file = Request.Form.Files[0];
            string path = _userService.UploadFile(file);

            if (path == "")
            {
                return StatusCode(500);
            }


            var obj = new
            {
                slikaPath = path
            };
            

            return Ok(obj);
        }


        [HttpPost("login")]
        public ActionResult Login(KorisnikLoginDto korisnik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = _userService.Login(korisnik);

            if (token == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public ActionResult Register(KorisnikRegisterDto korisnik)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registrovaniKorisnik = _userService.Register(korisnik);

            if (registrovaniKorisnik == null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Email adresa vec ima profil.");
            }

            return Ok(registrovaniKorisnik);
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("unactivated")]
        public ActionResult GetAllUnActivatedUsers()
        {
            return Ok(_userService.GetAllUnActivatedUsers());
        }

        [HttpGet("dostavljaci")]
        public ActionResult GetAllDostavljaci()
        {
            return Ok(_userService.GetAllDostavljaci());
        }

        [HttpGet]
        [Route("activate-user/{userId}")]
        [Authorize(Roles = "administrator")]
        public ActionResult ActivateUser(long userId)
        {
            var aktiviraniKorisnik = _userService.ActivateUser(userId);

            if (aktiviraniKorisnik == null)
            {
                return NotFound();
            }

            return Ok(aktiviraniKorisnik);
        }

        [HttpGet]
        [Route("{userId}")]
        public ActionResult GetUser(long userId)
        {
            var korisnik = _userService.GetUser(userId);

            if (korisnik == null)
            {
                return NotFound();
            }

            return Ok(korisnik);
        }


        [HttpPut]
        [Route("edit-profile/{userId}")]
        public ActionResult UpdateUser(long userId, [FromBody]KorisnikRegisterDto korisnik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var azuriraniKorisnik = _userService.UpdateUser(userId, korisnik);

            if (azuriraniKorisnik == null)
            {
                return NotFound();
            }

            return Ok(azuriraniKorisnik);
        }

        [HttpPut]
        [Route("change-profile-photo/{userId}")]
        public ActionResult ChangeProfilePhoto(long userId, IFormFile formFile)
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var filePath = _userService.UpdateProfilePhoto(userId, file);

                    var obj = new
                    {
                        slikaPath = filePath
                    };
                    return Ok(obj);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        
    }
}
