using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordStorageApi.Models
{
    public class PasswordModel
    {
        [Key]
        [Required]
        public int PasswordId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastUsedAt { get; set; }

        public bool IsCurrent { get; set; }
        public bool IsCompromised { get; set; }
        public bool IsDeleted { get; set; }

        //Navigation property
        public UserModel User { get; set; }
    }
}
