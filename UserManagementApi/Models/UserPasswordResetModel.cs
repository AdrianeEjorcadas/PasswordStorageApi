using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models
{
    public class UserPasswordResetModel
    {
        [Key]
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpirationDateTime { get; set; }

        [Required]
        public bool IsUsed { get; set; }

    }
}
