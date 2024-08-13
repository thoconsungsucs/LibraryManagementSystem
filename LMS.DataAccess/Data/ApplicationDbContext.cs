using LMS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().ToTable("Members");
            modelBuilder.Entity<Librarian>().ToTable("Librarians");
            base.OnModelCreating(modelBuilder);

            List<IdentityRole<int>> identityRoles = new List<IdentityRole<int>>
            {
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Librarian", NormalizedName = "LIBRARIAN" },
                new IdentityRole<int> { Id = 3, Name = "Member", NormalizedName = "MEMBER" },
            };

            modelBuilder.Entity<IdentityRole<int>>().HasData(identityRoles);
        }
    }
}
