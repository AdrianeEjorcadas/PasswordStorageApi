namespace UserManagementApi.Messages
{
    public static class ErrorMessages
    {
        //Exception message
        public const string ExceptionDefault = "An unexpected error occured.";

        // Token and session
        public const string InvalidToken = "Your authentication token is invalid or has expired. Please log in again to continue.";
        public const string ExpiredSession = "Your session has been expired. Please log in again to continue";
        public const string ResetTokenInvalid = "Invalid or missing reset token. Please ensure you are using the correct link or request a new password reset.";
        
        // User account
        public const string InvalidCredential = "The email address or password you entered is incorrect. Please try again.";
        public const string AccountLocked = "Your account has been locked due to security reasons. Please reset your password to regain access.";
        public const string InvalidConfirmationCredential = "The new password and confirmation password you entered do not match.";
    }
}
