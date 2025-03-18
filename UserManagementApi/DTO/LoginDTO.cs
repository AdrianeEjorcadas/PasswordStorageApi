using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string EmailAddress { get; set; }


        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

    }
}
