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
                .Include(n => n.NailServices)
                .FirstOrDefaultAsync(n => n.Id == id)
                ?? throw new Exception("Nail salon not found");
        }

        public async Task<IEnumerable<NailSalonImage>> GetNailSalonImagesAsync(int nailSalonId)
        {
            return await _context.NailSalonImages
                .Where(i => i.NailSalonId == nailSalonId)
                .ToListAsync();
        }

        public async Task<NailSalon> UpdateNailSalonAsync(int id, NailSalon nailSalon)
        {
            var existingNailSalon = await _context.NailSalons.FindAsync(id);
            if (existingNailSalon == null)
            {
                throw new Exception("Nail salon not found");
            }
            existingNailSalon.PhoneNumber = nailSalon.PhoneNumber;
            existingNailSalon.Address = nailSalon.Address;
            existingNailSalon.Name = nailSalon.Name;
            existingNailSalon.Description = nailSalon.Description;
            existingNailSalon.Email = nailSalon.Email;
            existingNailSalon.OpeningTime = nailSalon.OpeningTime;
            existingNailSalon.ClosingTime = nailSalon.ClosingTime;
            existingNailSalon.DaysOpen = nailSalon.DaysOpen;
            existingNailSalon.ImageUrl = nailSalon.ImageUrl;
            existingNailSalon.LocationId = nailSalon.LocationId;

            _context.Update(existingNailSalon);
            await _context.SaveChangesAsync();
            return existingNailSalon;
        }

        }
}
