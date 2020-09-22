using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerApp_Chess.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Username max length is 10 and minimum 5.", MinimumLength = 5)]
        public string Username { get; set; }
    }
}
