using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class Commands : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public Commands(ICommandRepo repo , IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!_repo.PlatformExits(platformId))
            {
                return NotFound();
            }

            var commands = _repo.GetCommandsForPlatform(platformId);
            var response = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(response);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public IActionResult GetCommandForPlatform(int platformId,int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId} / {commandId}");
            if (!_repo.PlatformExits(platformId))
            {
                return NotFound();
            }

            var command = _repo.GetCommand(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public IActionResult CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId} ");
            if (!_repo.PlatformExits(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);
            
            _repo.CreateCommand(platformId,command);
            _repo.SaveChange();

            var commandreadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId,commandId = commandreadDto.Id});
        }
    }
}