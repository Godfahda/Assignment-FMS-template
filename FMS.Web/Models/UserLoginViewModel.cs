using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using FMS.Data.Models;

namespace FMS.Web.Models
{
    public class UserLoginViewModel
    {       
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        public string Password { get; set; }

    }
}