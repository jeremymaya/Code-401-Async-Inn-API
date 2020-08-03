using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    .AddEntityFrameworkStores<AsyncInnDbContext>()
                    .AddDefaultTokenProviders();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

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
