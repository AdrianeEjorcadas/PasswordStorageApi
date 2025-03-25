namespace UserManagementApi.CustomExceptions
{
    public class RevokedTokenException : Exception
    {
        public RevokedTokenException(string message) : base(message)
        {
            
        }
    }
}
