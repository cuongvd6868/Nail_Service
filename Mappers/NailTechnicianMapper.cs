using Nail_Service.Models;
using Nail_Service.DTOs.NailTechnicianDto;
using Microsoft.Identity.Client;

namespace Nail_Service.Mappers
{
    public static class NailTechnicianMapper
    {
        public static NailTechnicianViewDto ToNailTechnicianViewDto(this NailTechnician nailTechnician)
        {
            return new NailTechnicianViewDto
            {
                Id = nailTechnician.Id,
                AppUserId = nailTechnician.AppUserId,
                FullName = nailTechnician.FullName,
                Bio = nailTechnician.Bio,
                ProfilePictureUrl = nailTechnician.ProfilePictureUrl,
                YearsOfExperience = nailTechnician.YearsOfExperience,
                Specialties = nailTechnician.Specialties,
                AverageRating = nailTechnician.AverageRating,
                NumberOfReviews = nailTechnician.NumberOfReviews,
                Status = nailTechnician.Status ?? "Available",
                IsActive = nailTechnician.IsActive ?? true,
                NailSalonId = nailTechnician.NailSalonId,
                AvailabilitySlots = nailTechnician.AvailabilitySlots ?? new List<AvailabilitySlot>(),
                NailSalon = nailTechnician.NailSalon
            };
        }

        public static NailTechnician ToNailTechnicianFromCreateDto(this CreateNailTechnicianDto createDto)
        {
            return new NailTechnician
            {
                AppUserId = createDto.AppUserId,
                FullName = createDto.FullName,
                Bio = createDto.Bio,
                ProfilePictureUrl = createDto.ProfilePictureUrl,
                YearsOfExperience = createDto.YearsOfExperience,
                Specialties = createDto.Specialties,
                Status = createDto.Status ?? "Busy",
                IsActive = createDto.IsActive ?? true,
                NailSalonId = createDto.NailSalonId
            };
        }

        public static NailTechnician ToNailTechnicianFromUpdate(this UpdateNailTechnicianDto updateDto)
        {
            return new NailTechnician
            {
                Id = updateDto.Id,
                AppUserId = updateDto.AppUserId,
                FullName = updateDto.FullName,
                Bio = updateDto.Bio,
                ProfilePictureUrl = updateDto.ProfilePictureUrl,
                YearsOfExperience = updateDto.YearsOfExperience,
                Specialties = updateDto.Specialties,
                NailSalonId = updateDto.NailSalonId
            };
        }

        public static NailTechnician ToUpdateStatusDto(this StatusNailTechnicianDto updateStatusDto)
        {
            return new NailTechnician
            {
                Status = updateStatusDto.Status ?? "Available",
            };
        }

    }
}
