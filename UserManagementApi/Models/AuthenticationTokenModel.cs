namespace UserManagementApi.Models
{
    public class AuthenticationTokenModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokeAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
