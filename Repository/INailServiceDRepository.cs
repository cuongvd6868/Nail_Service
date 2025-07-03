using Nail_Service.Models;

namespace Nail_Service.Repository
{
    public interface INailServiceDRepository
    {
        Task<IEnumerable<NailServiceD>> GetAllNailServiceD();
        Task<NailServiceD> GetNailServiceDById(int id);
        Task<NailServiceD> CreateNailServiceD(NailServiceD nailServiceD);
        Task<NailServiceD> UpdateNailServiceD(NailServiceD nailServiceD);
        Task DeleteNailServiceD(int id);
    }
}
