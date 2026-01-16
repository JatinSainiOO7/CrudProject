using System.ComponentModel.DataAnnotations;

namespace CrudProject.Models
{
    public class Student
    {
        [Key]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public int CourseId { get; set; }
        public string? Course { get; set; }
        public decimal Fees { get; set; }

        [Required]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
