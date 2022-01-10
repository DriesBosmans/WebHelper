using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEB_voorbereiding.Models;
using WEB_voorbereiding.ViewModels;

namespace WEB_voorbereiding.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lector> Lectors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<VakLector> VakLectors { get; set;}
        public DbSet<CustomIdentityUser> CustomIdentityUsers { get; set; }
        public DbSet<Vak> Vakken { get; set; }

    }
}
