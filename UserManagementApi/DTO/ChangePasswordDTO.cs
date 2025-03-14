using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class ChangePasswordDTO
    {

        [Required(ErrorMessage = "User name is required!")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "A valid username must have more than 6 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Old Password is required!")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A password should contain between 8 and 30 characters.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required!")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A password should contain between 8 and 30 characters")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation password is required!")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A password should contain between 8 and 30 characters")]
        public string ConfirmationPassword { get; set; }

    }
}
