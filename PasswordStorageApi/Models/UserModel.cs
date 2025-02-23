using System.ComponentModel.DataAnnotations;

namespace PasswordStorageApi.Models
{
    public class UserModel
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email address")]
        [StringLength(100, ErrorMessage = "UserName cannot be longer than 100 characters")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }

        //Navigation property
        public ICollection<PasswordModel> Passwords { get; set; }
    }
}
