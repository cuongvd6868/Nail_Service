using Nail_Service.Models;

namespace Nail_Service.Repository
{
    public interface INailSalonFavoriteRepository
    {
        Task<bool> AddToFavoriteAsync(int nailSalonId, string userId);
        Task<bool> RemoveFromFavoriteAsync(int nailSalonId, string userId);
        Task<bool> IsFavoriteAsync(int nailSalonId, string userId);
        Task<IEnumerable<NailSalonFavorite>> GetFavoritesByUserIdAsync(string userId);
    }
}
