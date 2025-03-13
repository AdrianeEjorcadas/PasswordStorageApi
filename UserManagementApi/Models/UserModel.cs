using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models
{
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User name is required!")]
        [StringLength(25, MinimumLength = 6, ErrorMessage ="User name must be more than 6 character")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password is required!")]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required(ErrorMessage ="Email is required!")]
        public string Email { get; set; }

        public bool IsActive { get; set; }
        
        public bool IsDeleted { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
