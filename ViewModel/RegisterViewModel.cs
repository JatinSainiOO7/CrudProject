using System.ComponentModel.DataAnnotations;

namespace CrudProject.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required.")]
        [StringLength(40, MinimumLength =8, ErrorMessage ="The {0} must be at least {2} and at max {1} character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match.")]

        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }
        public string UserRole { get; set; }


    }
}
