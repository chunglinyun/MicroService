using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class Commandsprofile : Profile
    {
        public Commandsprofile()
        {
            CreateMap<Platform, PlatfordReadDto>();
            CreateMap<CommandReadDto, Command>();
            CreateMap<Command, CommandReadDto>();
        }
    }
}