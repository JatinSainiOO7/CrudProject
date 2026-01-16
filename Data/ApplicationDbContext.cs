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


    }
}
