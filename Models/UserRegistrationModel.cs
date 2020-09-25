using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;

namespace BlazorServerApp_Chess.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Username max length is 10 and minimum 5.", MinimumLength = 5)]
        public string Username { get; set; }
        
        [Required]
        [PasswordPropertyText]
        [StringLength(20, ErrorMessage = "Password length must be 5 to 20 characters.", MinimumLength = 5)]
        public string Password { get; set; }
        
        [Required]
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "The password entered did not match.")]
        public string ConfirmPassword { get; set; }
    }
}
