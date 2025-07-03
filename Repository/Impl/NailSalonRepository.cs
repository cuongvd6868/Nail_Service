using Nail_Service.Data;
using Nail_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Nail_Service.Repository.Impl
{
    public class NailSalonRepository : INailSalonRepository
    {
        private readonly AppDbContext _context;

        public NailSalonRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<NailSalonImage> AddNailSalonImageAsync(NailSalonImage nailSalonImage)
        {
            throw new NotImplementedException();
        }

        public async Task<NailSalon> CreateNailSalonAsync(NailSalon nailSalon)
        {
            if (nailSalon == null)
            {
                throw new Exception("Nail salon cannot be null");
            }
            await _context.NailSalons.AddAsync(nailSalon);
            await _context.SaveChangesAsync();
            return nailSalon;

        }

        public async Task DeleteNailSalonAsync(int id)
        {
            var nailSalon = await _context.NailSalons.FindAsync(id);
            if (nailSalon == null)
            {
                throw new Exception("Nail salon not found");
            }
            _context.NailSalons.Remove(nailSalon);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<NailSalon>> GetAllNailSalonsAsync()
        {
            return await _context.NailSalons
                .Include(n => n.Amenities)
                .Include(n => n.NailTechnicians)
                .ToListAsync(); 
        }

        public async Task<NailSalon> GetNailSalonByIdAsync(int id)
        {
            return await _context.NailSalons
                .Include(n => n.Amenities)
                .Include(n => n.NailTechnicians)
                .Include(n => n.NailSalonImages)
                .FirstOrDefaultAsync(n => n.Id == id)
                ?? throw new Exception("Nail salon not found");
        }

        public async Task<IEnumerable<NailSalonImage>> GetNailSalonImagesAsync(int nailSalonId)
        {
            return await _context.NailSalonImages
                .Where(i => i.NailSalonId == nailSalonId)
                .ToListAsync();
        }

        public async Task<NailSalon> UpdateNailSalonAsync(NailSalon nailSalon)
        {
            if (nailSalon == null)
            {
                throw new Exception("Nail salon cannot be null");
            }
            var existingNailSalon = await _context.NailSalons.FindAsync(nailSalon.Id);
            if (existingNailSalon == null)
            {
                throw new Exception("Nail salon not found");
            }
            _context.Entry(existingNailSalon).CurrentValues.SetValues(nailSalon);
            await _context.SaveChangesAsync();
            return existingNailSalon;
        }
    }
}
