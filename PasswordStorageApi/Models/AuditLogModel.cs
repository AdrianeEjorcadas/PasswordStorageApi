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
        public long TimeStamp { get; set; }

        [Required]
        public string LogLevel { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        [Required]
        public string Message { get; set; }

        public string Exception { get; set; }

        [StringLength(50)]
        public string IpAddress { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

    }
}
