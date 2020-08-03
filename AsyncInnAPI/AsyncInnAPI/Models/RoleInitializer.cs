using System;
using System.Collections.Generic;
using System.Linq;
using AsyncInnAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public static void SeedData(IServiceProvider serviceProvider, UserManager<ApplicationUser> users, IConfiguration _config)
        {
            using var dbContext = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            dbContext.Database.EnsureCreated();
            AddRoles(dbContext);
            SeedUsers(users, _config);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager, IConfiguration _config)
        {
            if (userManager.FindByEmailAsync(_config["DistrictManagerEmail"]).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = _config["DistrictManagerEmail"],
                    Email = _config["DistrictManagerEmail"],
                    FirstName = "District",
                    LastName = "Manager"
                };

                IdentityResult result = userManager.CreateAsync(user, _config["DistrictManagerPassword"]).Result;

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
