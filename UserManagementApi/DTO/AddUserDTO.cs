using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class AddUserDTO
    {

        [Required(ErrorMessage = "User name is required!")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "User name must be more than 6 character")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        public string Email { get; set; }
    }
}
