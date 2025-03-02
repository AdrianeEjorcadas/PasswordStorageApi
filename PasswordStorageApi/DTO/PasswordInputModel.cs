using System.ComponentModel.DataAnnotations;

namespace PasswordStorageApi.DTO
{
    public class PasswordInputModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PlatformId { get; set; }
        [Required]
        public string PlainTextPassword { get; set; }
    }
}
