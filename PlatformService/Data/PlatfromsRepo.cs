using System;
using System.Collections.Generic;
using System.Linq;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatfromsRepo:IPlatfromRepo
    {
        private readonly AppDbContext _context;

        public PlatfromsRepo(AppDbContext context)
        {
            _context = context;
        }
        public bool SaveChange()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Platfrom> GetAllPlatfroms()
        {
            return _context.Platfroms.ToList();
        }

        public Platfrom GetPlatfromById(int id)
        {
            return _context.Platfroms.FirstOrDefault(x => x.Id == id);
        }

        public void CreatePlatfrom(Platfrom plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException();
            }

            _context.Platfroms.Add(plat);
        }
    }
}