namespace UserManagementApi.CustomExceptions
{
    public class TokenInvalidException : Exception
    {
        public int ErrorCode { get;}

        public TokenInvalidException(string message) : base(message)
        {
        }
    }
}
