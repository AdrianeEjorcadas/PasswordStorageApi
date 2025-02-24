using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordStorageApi.Models
{
    public class AuditLogModel
    {
        [Key]
        [Required]
        public int LogId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        [Required]
        public long UnixTimeStamp { get; set; }

        [Required]
        [StringLength(50)]
        public string IpAddress { get; set; }

        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }


        //Navigation property
        public UserModel User { get; set; }

    }
}
