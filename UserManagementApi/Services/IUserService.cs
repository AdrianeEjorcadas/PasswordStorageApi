﻿using UserManagementApi.DTO;
using UserManagementApi.Models;

namespace UserManagementApi.Services
{
    public interface IUserService
    {

        Task<bool> IsEmailExistingAsync(string email);
        Task<UserCredentialModel> CreateUserAsync(AddUserDTO addUserDTO);

        Task ValidateEmailTokenAsync(ConfirmationEmailDTO confirmationEmailDTO);

        Task ResendEmailTokenAsync(ResendConfirmationDTO resendConfirmationDTO);

        Task<UserCredentialModel> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

        Task<bool> ForgotPasswordAsync(string email);

        Task<UserCredentialModel> ResetPasswordAsync(string token, ResetPasswordDTO resetPasswordDTO);

        Task<AuthenticationTokenDetailsDTO> LoginAsync(LoginDTO loginDTO); 

        Task ValidateTokenAsync(string authToken);

        Task GenerateNewTokenAsync(string refreshToken);

        Task LogoutAsync(AuthenticationTokenDetailsDTO tokenDetailsDTO);
    }
}
