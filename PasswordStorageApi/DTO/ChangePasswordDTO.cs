using System.ComponentModel.DataAnnotations;

namespace PasswordStorageApi.DTO
{
    public class ChangePasswordDTO
    {

        [Required(ErrorMessage = "Password id is required")]
        public int PasswordId { get; set; }

        [Required(ErrorMessage ="Please input your old password")]
        [StringLength(100, ErrorMessage ="Password length should be more than 6 characters", MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please input your new password")]
        [StringLength(100, ErrorMessage = "Password length should be more than 6 characters", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please retype your new password")]
        [StringLength(100, ErrorMessage = "Password length should be more than 6 characters", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}
