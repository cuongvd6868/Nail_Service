namespace Nail_Service.Models
{
    public class NailSalonFavorite
    {
        public int Id { get; set; }
        public int NailSalonId { get; set; }
        public string UserId { get; set; }

        // Navigation properties
        public virtual NailSalon? NailSalon { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
