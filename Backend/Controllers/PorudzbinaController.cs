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
    public class PorudzbinaController : ControllerBase
    {
        private readonly IPorudzbinaService _porudzbinaService;

        public PorudzbinaController(IPorudzbinaService porudzbinaService)
        {
            _porudzbinaService = porudzbinaService;
        }

        // Samo admin
        [HttpGet]
        [Authorize(Roles = "administrator")]
        public ActionResult SvePorudzbine()
        {
            return Ok(_porudzbinaService.SvePorudzbine());
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "potrosac")]
        public ActionResult MojePorudzbine(long userId)
        {
            var mojePorudzbine = _porudzbinaService.MojePorudzbine(userId);

            if (mojePorudzbine == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(mojePorudzbine);
        }

        [HttpGet("potrosac-trenutna-porudzbina/{userId}")]
        [Authorize(Roles = "potrosac")]
        public ActionResult MojaTrenutnaPorudzbina(long userId)
        {
            //var mojePorudzbine = _porudzbinaService.MojePorudzbine(userId);
            var trenutnaPorudzbina = _porudzbinaService.TrenutnaPorudzbina(userId);

            if (trenutnaPorudzbina == null)
            {
                return NotFound("Ne postoji trenutna porudzbina potrosaca " + userId);
            }

            return Ok(trenutnaPorudzbina);
        }

        // Ovo koristi potrosac
        [HttpPost("{userId}")]
        [Authorize(Roles = "potrosac")]
        public ActionResult NovaPorudzbina(long userId, [FromBody] PorudzbinaDto porudzbina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var porudzbinaNova = _porudzbinaService.NovaPorudzbina(userId, porudzbina);

            if (porudzbinaNova == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(porudzbinaNova);
        }

        [HttpGet("dostavljac-poslednja-porudzbina/{porudzbinaId}")]
        [Authorize(Roles = "dostavljac")]
        public ActionResult DostavljacPoslednjaPorudzbina(long porudzbinaId)
        {
            var trenutnaPorudzbina = _porudzbinaService.DostavljacPoslednjaPorudzbina(porudzbinaId);

            if (trenutnaPorudzbina == null)
            {
                return NotFound("Ne postoji porudzbina  " + porudzbinaId);
            }

            return Ok(trenutnaPorudzbina);
        }

        [HttpGet("dostavljac-porudzbina/{porudzbinaId}")]
        [Authorize(Roles = "dostavljac")]
        public ActionResult DostavljacPorudzbina(long porudzbinaId)
        {
            var trenutnaPorudzbina = _porudzbinaService.DostavljacPorudzbina(porudzbinaId);

            if (trenutnaPorudzbina == null)
            {
                return NotFound("Ne postoji porudzbina  " + porudzbinaId);
            }

            return Ok(trenutnaPorudzbina);
        }

        [HttpGet("dostavljac-trenutna-porudzbina/{porudzbinaId}")]
        [Authorize(Roles = "dostavljac")]
        public ActionResult DostavljacTrenutnaPorudzbina(long porudzbinaId)
        {
            var trenutnaPorudzbina = _porudzbinaService.DostavljacTrenutnaPorudzbina(porudzbinaId);

            if (trenutnaPorudzbina == null)
            {
                return NotFound("Ne postoji trenutna porudzbina  " + porudzbinaId);
            }

            return Ok(trenutnaPorudzbina);
        }

        [HttpGet("dostavljac-nepotvrdjene-porudzbine")]
        [Authorize(Roles = "dostavljac")]
        public ActionResult DostavljacNepotvrdjenePorudzbine()
        {
            var nepotvrdjenePorudzbine = _porudzbinaService.SveNepotvrdjenePorudzbine();


            if (nepotvrdjenePorudzbine == null)
            {
                return NotFound();
            }

            return Ok(nepotvrdjenePorudzbine);
        }

        [HttpGet("potvrdi/{porudzbinaId}")]
        [Authorize(Roles = "dostavljac")]
        public long DostavljacPotvrdiPorudzbinu(long porudzbinaId)
        {
            return _porudzbinaService.PotvrdiPorudzbinu(porudzbinaId);
        }


        

        // Ovo koristi dostavljac
    }
}
