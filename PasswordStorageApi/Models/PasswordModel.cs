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
        [ForeignKey("PlatformId")]
        public int PlatformId { get; set; }

        [Required]
        public string EncryptedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsExpired { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastUsedAt { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }

        //Navigation property
        public UserModel User { get; set; }
        public PlatformModel Platform { get; set; }
    }
}
