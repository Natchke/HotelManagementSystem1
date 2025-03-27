using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Models.Entities;

namespace HotelManagement.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ApplicationUser>ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:1 Hotel ↔ Manager
            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Manager)
                .WithOne(m => m.Hotel)
                .HasForeignKey<Hotel>(h => h.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1:M Hotel ↔ Rooms
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // M:M Hotel ↔ Reservations (through join table)
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Reservations)
                .WithMany(r => r.Hotels)
                .UsingEntity<Dictionary<string, object>>(
                    "HotelReservations",
                    j => j.HasOne<Reservation>().WithMany().OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Hotel>().WithMany().OnDelete(DeleteBehavior.Restrict),
                    j => j.ToTable("HotelReservations"));

            // 1:M Room ↔ Reservations (corrected)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade

            // 1:M Guest ↔ Reservations
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}