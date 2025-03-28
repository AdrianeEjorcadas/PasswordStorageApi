using System.Runtime.Serialization;

namespace UserManagementApi.CustomExceptions
{
    [Serializable]
    public class AccountLockedException : Exception
    {
        public AccountLockedException()
            : base("The account has been locked")
        {
        }

        public AccountLockedException(string message) 
            : base(message)
        {
        }
        public AccountLockedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Serialization constructor (required for serialization)
        protected AccountLockedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
