using Nail_Service.DTOs.NailSalonDto;   
using Nail_Service.Models;

namespace Nail_Service.Mappers
{
    public static class NailSalonMapper
    {
        public static NailSalon ToNailSalonFromCreateDto(this CreateNailSalonDto createNailSalonDto)
        {
            return new NailSalon
            {
                Name = createNailSalonDto.Name,
                Address = createNailSalonDto.Address,
                PhoneNumber = createNailSalonDto.PhoneNumber,
                Email = createNailSalonDto.Email,
                Description = createNailSalonDto.Description,
                OpeningTime = createNailSalonDto.OpeningTime,
                ClosingTime = createNailSalonDto.ClosingTime,
                DaysOpen = createNailSalonDto.DaysOpen,
                ImageUrl = createNailSalonDto.ImageUrl,
                AverageRating = createNailSalonDto.AverageRating,
                NumberOfReviews = createNailSalonDto.NumberOfReviews,
                LocationId = createNailSalonDto.LocationId,
                Amenities = createNailSalonDto.Amenities ?? new List<Amenity>(),
                NailServices = createNailSalonDto.NailServices ?? new List<NailServiceD>(),
                NailTechnicians = createNailSalonDto.NailTechnicians ?? new List<NailTechnician>()
            };
        }

        public static NailSalon ToNailSalonFromUpdateDto(this UpdateNailSalonDto updateNailSalonDto)
        {
            return new NailSalon
            {
                Name = updateNailSalonDto.Name,
                Address = updateNailSalonDto.Address,
                PhoneNumber = updateNailSalonDto.PhoneNumber,
                Email = updateNailSalonDto.Email,
                Description = updateNailSalonDto.Description,
                OpeningTime = updateNailSalonDto.OpeningTime,
                ClosingTime = updateNailSalonDto.ClosingTime,
                DaysOpen = updateNailSalonDto.DaysOpen,
                ImageUrl = updateNailSalonDto.ImageUrl,
                AverageRating = updateNailSalonDto.AverageRating,
                NumberOfReviews = updateNailSalonDto.NumberOfReviews,
                LocationId = updateNailSalonDto.LocationId,
                Amenities = updateNailSalonDto.Amenities ?? new List<Amenity>(),
                NailServices = updateNailSalonDto.NailServices ?? new List<NailServiceD>(),
                NailTechnicians = updateNailSalonDto.NailTechnicians ?? new List<NailTechnician>()
            };
        }
    }
}
