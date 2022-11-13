using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChange();

        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatfromById(int id);
        void CreatePlatfrom(Platform plat);
    }
}