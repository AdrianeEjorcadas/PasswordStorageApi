using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models
{
    public class UserRolesModel
    {

        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        public UserCredentialModel User { get; set; }
        public RolesModel Role { get; set; }

    }
}
