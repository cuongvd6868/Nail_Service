using Nail_Service.Models;

namespace Nail_Service.Repository
{
    public interface INailSalonRepository
    {
        Task<IEnumerable<NailSalon>> GetAllNailSalonsAsync();
        Task<NailSalon> GetNailSalonByIdAsync(int id);
        Task<NailSalon> CreateNailSalonAsync(NailSalon nailSalon);
        Task<NailSalon> UpdateNailSalonAsync(int id, NailSalon nailSalon);
        Task DeleteNailSalonAsync(int id);
        Task<IEnumerable<NailSalonImage>> GetNailSalonImagesAsync(int nailSalonId);
        Task<NailSalonImage> AddNailSalonImageAsync(NailSalonImage nailSalonImage);
    }
}

