using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatfromRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatfromRepo repo, IMapper mapper,ICommandDataClient commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatfromReadDto>> GetPlatfrom()
        {
            var platfromItem = _repo.GetAllPlatfroms();
            return Ok(_mapper.Map<IEnumerable<PlatfromReadDto>>(platfromItem));
        }
        [HttpGet("{id}",Name = "GetPlatfromById")]
        public ActionResult<PlatfromReadDto> GetPlatfromById(int id)
        {
            var platfromItem = _repo.GetPlatfromById(id);
            if (platfromItem == null) return NotFound();
            return Ok(_mapper.Map<PlatfromReadDto>(platfromItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatfromReadDto>> CreatePlatfrom(PlatfromCreateDto item)
        {
            if (item == null) return BadRequest();
            var platfromModel = _mapper.Map<Platfrom>(item);

            _repo.CreatePlatfrom(platfromModel);
            _repo.SaveChange();

            var platfromReadDto = _mapper.Map<PlatfromReadDto>(platfromModel);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platfromReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously : {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatfromById), new {Id = platfromReadDto.Id}, platfromReadDto);
        }
    }
}