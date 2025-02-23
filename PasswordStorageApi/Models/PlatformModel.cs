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

        //navigation property
        public ICollection<PasswordModel> Passwords { get; set; }
    }
}
