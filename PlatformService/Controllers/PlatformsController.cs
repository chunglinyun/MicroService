using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
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
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper,ICommandDataClient commandDataClient,IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatfrom()
        {
            var platfromItem = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platfromItem));
        }
        [HttpGet("{id}",Name = "GetPlatfromById")]
        public ActionResult<PlatformReadDto> GetPlatfromById(int id)
        {
            var platfromItem = _repo.GetPlatfromById(id);
            if (platfromItem == null) return NotFound();
            return Ok(_mapper.Map<PlatformReadDto>(platfromItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatfrom(PlatfromCreateDto item)
        {
            if (item == null) return BadRequest();
            var platfromModel = _mapper.Map<Platform>(item);

            _repo.CreatePlatfrom(platfromModel);
            _repo.SaveChange();

            var platfromReadDto = _mapper.Map<PlatformReadDto>(platfromModel);
            
            //Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platfromReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously : {ex.Message}");
            }

            //Send Async Message

            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platfromReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously : {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatfromById), new {Id = platfromReadDto.Id}, platfromReadDto);
        }
    }
}