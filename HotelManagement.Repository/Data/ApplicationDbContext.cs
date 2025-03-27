using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


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
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:1 - Hotel ↔ Manager (One hotel has one manager, one manager manages one hotel)
            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Manager)
                .WithOne(m => m.Hotel)
                .HasForeignKey<Hotel>(h => h.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:M - Hotel ↔ Rooms (One hotel has many rooms, each room belongs to one hotel)
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // M:M - Hotel ↔ Reservation (Many hotels can have many reservations)
            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Reservations)
                .WithMany(r => r.Hotels)
                .UsingEntity<Dictionary<string, object>>(
                    "HotelReservations",
                    j => j.HasOne<Reservation>().WithMany().HasForeignKey("ReservationId"),
                    j => j.HasOne<Hotel>().WithMany().HasForeignKey("HotelId"),
                    j => j.ToTable("HotelReservations")
                );

            // 1:M - Room ↔ Reservation (One room can have many reservations)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:M - Guest ↔ Reservation (One guest can have many reservations)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
