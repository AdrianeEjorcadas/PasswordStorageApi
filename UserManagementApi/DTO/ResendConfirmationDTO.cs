using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class ResendConfirmationDTO
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required]
        public string Email { get; set; }
    }
}
