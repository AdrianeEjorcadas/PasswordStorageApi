using System.ComponentModel.DataAnnotations;

namespace PasswordStorageApi.Models
{
    public class PlatformModel
    {
        [Key]
        [Required]
        public int PlatformId { get; set; }

        [StringLength(50)]
        public string? PlatformName { get; set; }
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }

        //navigation property
        //public ICollection<PasswordModel> Passwords { get; set; }
    }
}
