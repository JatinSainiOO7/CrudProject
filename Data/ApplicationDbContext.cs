using CrudProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrudProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentUsers> StudentsUsers { get; set; }
        public DbSet<CourseManagement> CourseManagement { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for Fees
            modelBuilder.Entity<CourseManagement>()
                .Property(c => c.Fees)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Student>()
                .Property(s => s.Fees)
                .HasPrecision(18, 2);
        }
    }
}
