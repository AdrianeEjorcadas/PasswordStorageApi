using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class ResetPasswordDTO
    {

        [Required(ErrorMessage = "New password is required!")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A password should contain between 8 and 30 characters")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation password is required!")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A password should contain between 8 and 30 characters")]
        public string ConfirmPassword { get; set; }
    }
}
