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
        public DbSet<Member> Members { get; set; }
        public DbSet<Librarian> Librarians { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().ToTable("Members");
            modelBuilder.Entity<Librarian>().ToTable("Librarians");
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> identityRoles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Librarian", NormalizedName = "LIBRARIAN" },
                new IdentityRole { Name = "Member", NormalizedName = "MEMBER" },
            };
            modelBuilder.Entity<IdentityRole>().HasData(identityRoles);
        }
    }
}
