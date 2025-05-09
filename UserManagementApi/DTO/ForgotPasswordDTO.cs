﻿using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.DTO
{
    public class ForgotPasswordDTO
    {

        [Required(ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string EmailAddress { get; set; }
    }
}
