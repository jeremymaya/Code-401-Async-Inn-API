using System;
using AsyncInnAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AsyncInnAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=db;Database=AsyncInnDbContext;User=sa;Password=Kim12341234!");
        //docker run --name psql -e POSTGRES_USER=sa -e POSTGRES_PASSWORD=Kim12341234! -d postgres
    }
}
