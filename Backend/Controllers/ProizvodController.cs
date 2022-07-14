using Backend.Dto;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProizvodController : ControllerBase
    {
        private readonly IProizvodService _service;

        public ProizvodController(IProizvodService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult GetAllProizvodi()
        {
            return Ok(_service.GetAllProizvodi());
        }

        [HttpGet("{idProizvoda}")]
        public ActionResult GetProizvod(long idProizvoda)
        {
            var proizvod = _service.GetProizvod(idProizvoda);

            if (proizvod == null)
            {
                return NotFound();
            }

            return Ok(proizvod);
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult AddNewProizvod(ProizvodDto proizvod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dodatiProizvod = _service.AddNewProizvod(proizvod);

            if (dodatiProizvod == null)
            {
                return StatusCode(StatusCodes.Status409Conflict, $"Vec postoji proizvod sa imenom {proizvod.Ime}");
            }

            return Ok(dodatiProizvod);
        }

        [HttpDelete("RemoveProizvod/{idProizvoda}")]
        //[Authorize(Roles = "administrator")]
        public ActionResult RemoveProizvod(long idProizvoda)
        {
            _service.RemoveProizvod(idProizvoda);

            return Ok();
        }

    }
}
