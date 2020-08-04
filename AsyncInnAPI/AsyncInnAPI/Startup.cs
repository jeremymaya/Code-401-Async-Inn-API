using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AsyncInnAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IHostEnvironment environment)
        {
            Environment = environment;
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Enables the use of MVC controllers
            services.AddMvc();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            string applicationUserDbContextConnectionString = Environment.IsDevelopment()
                ? Configuration["ConnectionStrings:ApplicationUserDbContextDevelopmentConnection"]
                : Configuration["ConnectionStrings:ApplicationUserDbContextProductionConnection"];

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationUserDbContextConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWTIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTKey"]))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DistrictManagerPrivilege", policy => policy.RequireRole(ApplicationRoles.DistrictManager));
                options.AddPolicy("PropertyManagerPrivilege", policy => policy.RequireRole(ApplicationRoles.DistrictManager,
                                                                                           ApplicationRoles.PropertyManager));
                options.AddPolicy("AgentPrivilege", policy => policy.RequireRole(ApplicationRoles.DistrictManager,
                                                                                 ApplicationRoles.PropertyManager,
                                                                                 ApplicationRoles.Agent));
            });

            string asyncInnDbContextConnectionString = Environment.IsDevelopment()
                ? Configuration["ConnectionStrings:AsyncInnDbContextDevelopmentConnection"]
                : Configuration["ConnectionStrings:AsyncInnDbContextProductionConnection"];

            services.AddDbContext<AsyncInnDbContext>(options => options.UseSqlServer(asyncInnDbContextConnectionString));

            services.AddTransient<IHotelManager, HotelManager>();

            services.AddTransient<IRoomManager, RoomManager>();

            services.AddTransient<IAmenityManager, AmenityManager>();

            services.AddTransient<IHotelRoomManager, HotelRoomManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            RoleInitializer.SeedData(serviceProvider, userManager, Configuration);


            // Sets the default routing for incoming requests within the API application
            // By default, the convention is {site}/[controller]/[action]/[id]
            // id is not required, allowing it to be nullable
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
