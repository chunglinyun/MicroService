using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatfromRepo
    {
        bool SaveChange();

        IEnumerable<Platform> GetAllPlatfroms();
        Platform GetPlatfromById(int id);
        void CreatePlatfrom(Platform plat);
    }
}