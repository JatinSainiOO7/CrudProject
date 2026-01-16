using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CrudProject.Models
{
    public class Users : IdentityUser
    {
        public string UserRole { get; set; }
        public string IsActive { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

    }
}
