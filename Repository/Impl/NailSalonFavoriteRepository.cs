using Nail_Service.Data;
using Nail_Service.Models;
using Microsoft.EntityFrameworkCore;
using Nail_Service.DTOs.SalonFavoriteDto;

namespace Nail_Service.Repository.Impl
{
    public class NailSalonFavoriteRepository : INailSalonFavoriteRepository
    {
        private readonly AppDbContext _context;

        public NailSalonFavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToFavoriteAsync(int nailSalonId, string userId)
        {
            var favorite = new NailSalonFavorite
            {
                NailSalonId = nailSalonId,
                UserId = userId
            };
            await _context.NailSalonFavorites.AddAsync(favorite);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<NailSalonFavorite>> GetFavoritesByUserIdAsync(string userId)
        {
            return await _context.NailSalonFavorites
                .Where(f => f.UserId == userId)
                .Select(f => new NailSalonFavorite
                {
                    NailSalon = _context.NailSalons.FirstOrDefault(n => n.Id == f.NailSalonId)
                })
                .ToListAsync();
        }


        public async Task<bool> IsFavoriteAsync(int nailSalonId, string userId)
        {
            return await _context.NailSalonFavorites
                .AnyAsync(f => f.NailSalonId == nailSalonId && f.UserId == userId);
        }

        public async Task<bool> RemoveFromFavoriteAsync(int nailSalonId, string userId)
        {
            var favoriteRemove = await _context.NailSalonFavorites
                .FirstOrDefaultAsync(f => f.NailSalonId == nailSalonId && f.UserId == userId);
            if (favoriteRemove != null)
            {
                _context.NailSalonFavorites.Remove(favoriteRemove);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
