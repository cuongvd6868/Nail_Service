using Nail_Service.Models;

namespace Nail_Service.Repository
{
    public interface INailTechnicianRepository
    {
        Task<IEnumerable<NailTechnician>> GetAllTechniciansAsync();
        Task<NailTechnician> GetTechnicianByIdAsync(int id);
        Task<NailTechnician> CreateTechnicianAsync(NailTechnician technician);
        Task<NailTechnician> UpdateTechnicianAsync(string userId, int id, NailTechnician technician);
        Task DeleteTechnicianAsync(int id);
        Task<IEnumerable<NailTechnician>> GetTechniciansBySalonIdAsync(int salonId);
        Task<bool> UpdateStatusAsync(string userId, int id, NailTechnician technician);

    }
}
