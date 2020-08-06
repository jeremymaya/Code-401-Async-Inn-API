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
        public const string DistrictManager = "District Manager";
        public const string PropertyManager = "Property Manager";
        public const string Agent = "Agent";
    }
}
