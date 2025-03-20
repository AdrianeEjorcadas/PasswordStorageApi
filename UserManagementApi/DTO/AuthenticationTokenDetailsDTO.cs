namespace UserManagementApi.DTO
{
    public class AuthenticationTokenDetailsDTO
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AuthTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

    }
}
