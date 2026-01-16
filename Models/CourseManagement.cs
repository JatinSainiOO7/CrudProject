using System.ComponentModel.DataAnnotations;

namespace CrudProject.Models
{
    public class CourseManagement
    {
        [Key]
        public int CourseId { get; set; }
         public string CourseName { get; set; }
        public string Semester { get; set; }

        public decimal Fees { get; set; }
    }
}
