namespace UserManagementApi.Messages
{
    public static class ErrorMessages
    {
        //Exception message
        public const string ExceptionDefault = "An unexpected error occured.";

        // Token and session
        public const string InvalidToken = "Your authentication token is invalid or has expired. Please log in again to continue.";
        public const string ExpiredToken = "Your token is invalid or has expired. Please try again or contact your administrator.";
        public const string ExpiredSession = "Your session has been expired. Please log in again to continue";
        public const string ResetTokenInvalid = "Invalid or missing reset token. Please ensure you are using the correct link or request a new password reset.";
        public const string ConfirmationTokenInvalid = "Invalid or missing confirmation token. Please ensure you are using the correct link.";
        public const string TokenNotRegistered = "Failed to register the reset token.";
        
        // User account
        public const string InvalidCredential = "The email address or password you entered is incorrect. Please try again.";
        public const string InvalidUser = "Invalid user. Please log in again to continue.";
        public const string AccountLocked = "Your account has been locked due to security reasons. Please reset your password to regain access.";
        public const string InvalidConfirmationCredential = "The new password and confirmation password you entered do not match.";
        public const string InvalidPassword = "The password you have entered does not match our records. Please verify your input and try again.";
        public const string EmailExist = "The email address you entered is already associated with an account.\nPlease use a different email address or log in to your existing account.";
        public const string UsernameExist = "The username you entered is already associated with an account.\nPlease use a different email address or log in to your existing account.";

    }
}
