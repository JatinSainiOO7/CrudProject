using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CrudProject.Models
{
    public class SchoolUser 
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get;set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        
        [Required (ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        public string IsActive { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

    }
}
