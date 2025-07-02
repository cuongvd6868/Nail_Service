using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Nail_Service.Models
{
    public class BookingNail
    {
        public int Id { get; set; } 

        [Required]
        public DateTime BookingDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; } 
        public BookingStatus Status { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string CustomerId { get; set; }
        public int NailTechnicianId { get; set; }
        public int NailSalonId { get; set; }

        //navigation properties
        [ForeignKey("NailTechnicianId")] 
        public NailTechnician? NailTechnician { get; set; } 
        [ForeignKey("CustomerId")] 
        public AppUser? Customer { get; set; } 
        [ForeignKey("NailSalonId")]
        public NailSalon? NailSalon { get; set; }
        public Payment? Payment { get; set; }
        public List<Promotion>? Promotions { get; set; }
        public List<NailServiceD>? NailServices { get; set; } = new List<NailServiceD>();
        public List<Reviews>? Reviews { get; set; } = new List<Reviews>();
        public List<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot> { };
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Canceled,
        NoShow,
        Rescheduled
    }

}