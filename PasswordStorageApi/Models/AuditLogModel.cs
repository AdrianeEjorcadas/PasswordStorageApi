using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordStorageApi.Models
{
    public class AuditLogModel
    {
        [Key]
        [Required]
        public int LogId { get; set; }

        // Logger category name
        [StringLength(50)]
        public string? Category { get; set; }

        // Log message
        [StringLength(1000)]
        public string? Message { get; set; }

        // Log level (information, warning, error, etc.)
        [Required]
        public string LogLevel { get; set; }

        //Exception details
        public string? Exception { get; set; }

        //Log creation 
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //[Required]
        //[StringLength(50)]
        //public string UserName { get; set; }

    }
}
