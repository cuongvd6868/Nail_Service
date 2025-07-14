using Microsoft.EntityFrameworkCore;
using Nail_Service.Data;
using Nail_Service.Models;

namespace Nail_Service.Repository.Impl
{
    public class NailTechnicianRepository : INailTechnicianRepository
    {
        private readonly AppDbContext _context;

        public NailTechnicianRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<NailTechnician> CreateTechnicianAsync(NailTechnician technician)
        {
            if(technician == null)
            {
                throw new ArgumentNullException(nameof(technician), "Technician cannot be null.");
            }
            await _context.NailTechnicians.AddAsync(technician);
            await _context.SaveChangesAsync();
            return technician;
        }

        public async Task<NailTechnician> UpdateTechnicianAsync(string userId, int id, NailTechnician technician)
        {
            var existingTechnician = await _context.NailTechnicians
                .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);
            if (existingTechnician == null)
            {
                throw new KeyNotFoundException($"Nail technician with ID {id} not found for user {userId}.");
            }
            existingTechnician.FullName = technician.FullName;
            existingTechnician.Bio = technician.Bio;
            existingTechnician.ProfilePictureUrl = technician.ProfilePictureUrl;
            existingTechnician.YearsOfExperience = technician.YearsOfExperience;
            existingTechnician.Specialties = technician.Specialties;
            existingTechnician.NailSalonId = technician.NailSalonId;

            _context.NailTechnicians.Update(existingTechnician);
            await _context.SaveChangesAsync();
            return existingTechnician;
        }
        public async Task DeleteTechnicianAsync(int id)
        {
            var technician = await _context.NailTechnicians.FindAsync(id);
            if (technician == null)
            {
                throw new KeyNotFoundException($"Nail technician with ID {id} not found.");
            }
            _context.NailTechnicians.Remove(technician);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NailTechnician>> GetAllTechniciansAsync()
        {
            return await _context.NailTechnicians
                .Include(t => t.NailSalon)
                .ToListAsync();
        }

        public async Task<NailTechnician> GetTechnicianByIdAsync(int id)
        {
            return await _context.NailTechnicians
                .Include(t => t.NailSalon)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<NailTechnician>> GetTechniciansBySalonIdAsync(int salonId)
        {
            return await _context.NailTechnicians
                .Where(t => t.NailSalonId == salonId)
                .Include(t => t.NailSalon)
                .ToListAsync();
        }

        public async Task<IEnumerable<NailTechnician>> getNailTechniciansBySalonIdAsync(int salonId)
        {
            return await _context.NailTechnicians
                .Where(t => t.NailSalonId == salonId)
                .Include(t => t.NailSalon)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(string userId, int id, NailTechnician technician)
        {
            var nailTechnician = await _context.NailTechnicians
                .FirstOrDefaultAsync(t => t.Id == id && t.AppUserId == userId);

            if (nailTechnician == null)
            {
                throw new KeyNotFoundException($"Nail technician with ID {id} not found for user {userId}.");
            }

            nailTechnician.Status = technician.Status;

            return await _context.SaveChangesAsync() > 0;
        }


    }
}