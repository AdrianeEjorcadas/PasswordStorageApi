using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models
{
    public class UserCredentialModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Email { get; set; }

        public int FailedLoginAttempts { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string? ConfirmationToken { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ConfirmationTokenExpiration { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
