namespace UserManagementApi.CustomExceptions
{
    public class AccountLockedException : Exception
    {
        public AccountLockedException(string message) : base(message)
        {}
    }
}
