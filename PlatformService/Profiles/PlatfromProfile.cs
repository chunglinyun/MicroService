using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatfromProfile : Profile
    {
        public PlatfromProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatfromCreateDto,Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
        }
    }
}