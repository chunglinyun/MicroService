using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatfromRepo
    {
        bool SaveChange();

        IEnumerable<Platfrom> GetAllPlatfroms();
        Platfrom GetPlatfromById(int id);
        void CreatePlatfrom(Platfrom plat);
    }
}