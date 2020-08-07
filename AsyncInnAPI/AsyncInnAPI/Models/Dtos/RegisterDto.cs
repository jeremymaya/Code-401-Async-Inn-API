using System;
using System.ComponentModel.DataAnnotations;

namespace AsyncInnAPI.Models.Dtos
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
