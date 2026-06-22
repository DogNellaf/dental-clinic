using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DentalClinic.Models;

namespace DentalClinic
{
    public class DatabaseContext : IdentityDbContext<Profile, Role, long>
    {
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Staff> Staffs { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        //public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<long>>(b =>
            {
                b.HasKey(iul => new { iul.LoginProvider, iul.ProviderKey });
            });

            modelBuilder.Entity<IdentityUserRole<long>>(b =>
            {
                b.HasKey(iur => new { iur.UserId, iur.RoleId });
            });

            modelBuilder.Entity<IdentityUserToken<long>>(b =>
            {
                b.HasKey(iut => new { iut.UserId, iut.LoginProvider, iut.Name });
            });

            modelBuilder.Entity<IdentityRoleClaim<long>>(b =>
            {
                b.HasKey(irc => irc.Id);
            });
                
            modelBuilder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(iuc => iuc.Id);
            });

            modelBuilder.Entity<Service>().HasMany(e => e.Staff).WithMany(e => e.Services);
            modelBuilder.Entity<Service>().HasMany(e => e.Appointments).WithMany(e => e.Services);
        }
    }
}
