using Microsoft.EntityFrameworkCore;
using Nail_Service.Data;
using Nail_Service.Models;

namespace Nail_Service.Repository.Impl
{
    public class NailServiceDRepository : INailServiceDRepository
    {
        private readonly AppDbContext _context;
        public NailServiceDRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NailServiceD> CreateNailServiceD(NailServiceD nailServiceD)
        {
            if(nailServiceD == null)
            {
                throw new ArgumentNullException(nameof(nailServiceD), "Nail service cannot be null");
            }
            await _context.NailServices.AddAsync(nailServiceD);
            await _context.SaveChangesAsync();
            return nailServiceD;
        }

        public async Task DeleteNailServiceD(int id)
        {
            var nailService = await _context.NailServices.FindAsync(id);
            if (nailService == null)
            {
                throw new KeyNotFoundException($"Nail service with ID {id} not found");
            }
            _context.NailServices.Remove(nailService);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NailServiceD>> GetAllNailServiceD()
        {
            return await _context.NailServices.ToListAsync();
        }

        public Task<List<NailServiceD>> GetAllServicesBySalonId(int salonId)
        {
            return _context.NailServices
                .Where(n => n.NailSalons.Any(s => s.Id == salonId))
                .ToListAsync();
        }

        public async Task<NailServiceD> GetNailServiceDById(int id)
        {
            return await _context.NailServices.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<NailServiceD>> GetServicesBYIdAsync(List<int> serviceIds)
        {
            return await _context.NailServices
                .Where(n => serviceIds.Contains(n.Id))
                .ToListAsync();
        }

        public async Task<NailServiceD> UpdateNailServiceD(NailServiceD nailServiceD)
        {
            if (nailServiceD == null)
            {
                throw new ArgumentNullException(nameof(nailServiceD), "Nail service cannot be null");
            }
            var existingService = await _context.NailServices.FindAsync(nailServiceD.Id);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Nail service with ID {nailServiceD.Id} not found");
            }
            _context.Entry(existingService).CurrentValues.SetValues(nailServiceD);
            await _context.SaveChangesAsync();
            return existingService;
        }
    }
}
