namespace UserManagementApi.CustomExceptions
{
    public class InvalidTokenException : Exception
    {
        public int ErrorCode { get;}

        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}
