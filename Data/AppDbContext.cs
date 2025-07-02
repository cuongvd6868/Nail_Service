using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nail_Service.Models;

namespace Nail_Service.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<NailSalon> NailSalons { get; set; }
        public DbSet<NailTechnician> NailTechnicians { get; set; }
        public DbSet<NailServiceD> NailServices { get; set; }
        public DbSet<NailSalonImage> NailSalonImages { get; set; }
        public DbSet<BookingNail> Bookings { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<NailSalonFavorite> NailSalonFavorites { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<NailServiceDCategory> NailServiceDCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Amenity>().
                HasMany(n => n.NailSalons)
                .WithMany(n => n.Amenities)
                .UsingEntity(j => j.ToTable("NailSalonAmenities"));

            modelBuilder.Entity<AvailabilitySlot>()
                .HasOne(n => n.NailTechnician)
                .WithMany(n => n.AvailabilitySlots)
                .HasForeignKey(n => n.NailTechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AvailabilitySlot>()
                .HasOne(n => n.BookingNail)
                .WithMany(b => b.AvailabilitySlots)
                .HasForeignKey(n => n.BookingNailId)
                .OnDelete(DeleteBehavior.SetNull); 


            modelBuilder.Entity<BookingNail>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId);

            modelBuilder.Entity<BookingNail>()
                .HasOne(b => b.Customer)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<BookingNail>()
                .HasOne(b => b.NailSalon)
                .WithMany(n => n.Bookings)
                .HasForeignKey(b => b.NailSalonId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<BookingNail>()
                .HasOne(b => b.NailTechnician)
                .WithMany(n => n.BookingNails)
                .HasForeignKey(b => b.NailTechnicianId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BookingNail>()
                .HasMany(b => b.Promotions)
                .WithMany(p => p.BookingNails)
                .UsingEntity(j => j.ToTable("BookingPromotion"));

            modelBuilder.Entity<BookingNail>()
                .HasMany(b => b.NailServices)
                .WithMany(n => n.Bookings)
                .UsingEntity(j => j.ToTable("BookingNailService"));

            modelBuilder.Entity<BookingNail>()
                .HasMany(b => b.NailServices)
                .WithMany(n => n.Bookings)
                .UsingEntity(j => j.ToTable("BookingNailService"));

            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.BookingNail)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookingNailId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.NailSalon)
                .WithMany(n => n.Reviews)
                .HasForeignKey(r => r.NailSalonId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Location>()
                .HasMany(l => l.NailSalons)
                .WithOne(n => n.Location)
                .HasForeignKey(n => n.LocationId);

            modelBuilder.Entity<NailSalonImage>()
                .HasOne(n => n.NailSalon)
                .WithMany(n => n.NailSalonImages)
                .HasForeignKey(n => n.NailSalonId);

            modelBuilder.Entity<NailSalon>()
                .HasMany(n => n.NailServices)
                .WithMany(ns => ns.NailSalons)
                .UsingEntity(j => j.ToTable("NailSalonNailService"));

            modelBuilder.Entity<NailSalon>()
                .HasMany(n => n.NailTechnicians)
                .WithOne(nt => nt.NailSalon)
                .HasForeignKey(nt => nt.NailSalonId);

            modelBuilder.Entity<NailSalonFavorite>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.NailSalon)
                      .WithMany(h => h.NailSalonFavorites)
                      .HasForeignKey(e => e.NailSalonId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.NailSalonFavorites)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.ToTable("NailSalonFavorites");
            });

            modelBuilder.Entity<NailServiceD>()
                .HasOne(n => n.NailServiceDCategory)
                .WithMany(c => c.NailServiceDs)
                .HasForeignKey(n => n.NailServiceDCategoryId);

            modelBuilder.Entity<NailTechnician>()
                .HasOne(n => n.AppUser)
                .WithOne(u => u.NailTechnicianProfile)
                .HasForeignKey<NailTechnician>(n => n.AppUserId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId);

        }
    }
}
