using System;
using Microsoft.AspNetCore.Identity;

namespace AsyncInnAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class ApplicationRoles
    {
        public const string DistrictManager = "DistrictManager";
        public const string PropertyManager = "PropertyManager";
        public const string Agent = "Agent";
    }
}
