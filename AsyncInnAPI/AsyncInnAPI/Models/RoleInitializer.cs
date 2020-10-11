using System;
using System.Collections.Generic;
using System.Linq;
using AsyncInnAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AsyncInnAPI.Models
{
    public class RoleInitializer
    {
        private static readonly List<IdentityRole> Roles = new List<IdentityRole>()
        {
            new IdentityRole
            {
                Name = ApplicationRoles.DistrictManager,
                NormalizedName = ApplicationRoles.DistrictManager.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },

            new IdentityRole
            {
                Name = ApplicationRoles.PropertyManager,
                NormalizedName = ApplicationRoles.PropertyManager.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },

            new IdentityRole
            {
                Name = ApplicationRoles.Agent,
                NormalizedName = ApplicationRoles.Agent.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        };

        public static void SeedData(IServiceProvider serviceProvider, UserManager<ApplicationUser> users, IConfiguration _configuration, IWebHostEnvironment _webHostEnvironment)
        {
            using var dbContext = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            //dbContext.Database.EnsureCreated();
            AddRoles(dbContext);
            SeedUsersAsync(users, _configuration, _webHostEnvironment);
        }

        private static void SeedUsersAsync(UserManager<ApplicationUser> userManager, IConfiguration _configuration, IWebHostEnvironment _webHostEnvironment)
        {
            string districtManagerEmail = _webHostEnvironment.IsDevelopment()
                ? _configuration["DISTRICT_MANAGER_EMAIL"]
                : Environment.GetEnvironmentVariable("DISTRICT_MANAGER_EMAIL");

            if (userManager.FindByEmailAsync(districtManagerEmail).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = districtManagerEmail,
                    Email = districtManagerEmail,
                    FirstName = "Default",
                    LastName = "Manager"
                };

                string districtManagerPassword = _webHostEnvironment.IsDevelopment()
                    ? _configuration["DISTRICT_MANAGER_PASSWORD"]
                    : Environment.GetEnvironmentVariable("DISTRICT_MANAGER_PASSWORD");

                IdentityResult result = userManager.CreateAsync(user, districtManagerPassword).Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, ApplicationRoles.DistrictManager).Wait();
            }
        }

        private static void AddRoles(ApplicationDbContext dbContext)
        {
            if (dbContext.Roles.Any()) return;

            foreach (var role in Roles)
            {
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
        }
    }
}
