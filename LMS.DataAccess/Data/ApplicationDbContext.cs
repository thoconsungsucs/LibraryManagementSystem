using LMS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> identityRoles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Librarian", NormalizedName = "LIBRARIAN" },
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" },
            };
            modelBuilder.Entity<IdentityRole>().HasData(identityRoles);
        }
    }
}
