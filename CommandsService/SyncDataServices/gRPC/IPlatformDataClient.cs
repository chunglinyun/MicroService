using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.SyncDataServices.gRPC
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}