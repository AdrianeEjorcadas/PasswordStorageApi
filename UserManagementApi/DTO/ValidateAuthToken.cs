namespace UserManagementApi.DTO
{
    public class ValidateAuthToken
    {
        public Guid UserId { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AuthTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public bool IsRevoked { get; set; }
    }
}
