using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class ConfirmationEmailDTO
    {
        public string ConfirmationToken { get; set; }
    }
}
