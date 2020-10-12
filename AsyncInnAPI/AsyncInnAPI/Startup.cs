using System;
using System.IO;
using System.Reflection;
using System.Text;
using AsyncInnAPI.Data;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Interfaces;
using AsyncInnAPI.Models.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AsyncInnAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IWebHostEnvironment webHostEnvironment)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
            WebHostEnvironment = webHostEnvironment;
        }

        // Source: https://n1ghtmare.github.io/2020-09-28/deploying-a-dockerized-aspnet-core-app-using-a-postgresql-db-to-heroku/
        private string GetHerokuConnectionString(string connectionString)
        {
            string connectionUrl = WebHostEnvironment.IsDevelopment()
                ? Configuration["ConnectionStrings:" + connectionString]
                : Environment.GetEnvironmentVariable(connectionString);

            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable AuthorizeFilter across the application
            // Enable the use of controllers within the MVC convention
            services.AddControllers(options => { options.Filters.Add(new AuthorizeFilter()); })
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(GetHerokuConnectionString("USER_ROSE_URL_ENV")));

            // Enable Identity based on ApplicationUsr and IdentityRole
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            string jwtKey = WebHostEnvironment.IsDevelopment()
                ? Configuration["JWT_KEY"]
                : Environment.GetEnvironmentVariable("JWT_KEY");

            string jwtIssuer = WebHostEnvironment.IsDevelopment()
                ? Configuration["JWT_ISSUER"]
                : Environment.GetEnvironmentVariable("JWT_ISSUER");

            // Enable Authentication with JWT
            // Define JWT Beaere defaults and parameters
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
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            // Enable Authoriziation by adding custom policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DistrictManagerPrivilege", policy => policy.RequireRole(ApplicationRoles.DistrictManager));
                options.AddPolicy("PropertyManagerPrivilege", policy => policy.RequireRole(ApplicationRoles.DistrictManager,
                                                                                           ApplicationRoles.PropertyManager));
                options.AddPolicy("AgentPrivilege", policy => policy.RequireRole(ApplicationRoles.DistrictManager,
                                                                                 ApplicationRoles.PropertyManager,
                                                                                 ApplicationRoles.Agent));
            });
            
            services.AddDbContext<AsyncInnDbContext>(options => options.UseNpgsql(GetHerokuConnectionString("ASYNC_INN_BRONZE_URL_ENV")));

            services.AddTransient<IHotelManager, HotelManager>();

            services.AddTransient<IRoomManager, RoomManager>();

            services.AddTransient<IAmenityManager, AmenityManager>();

            services.AddTransient<IHotelRoomManager, HotelRoomManager>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Async Inn API",
                    Description = "A simple ASP.NET Core Web API for Async Inn Hotel Management",
                    Contact = new OpenApiContact
                    {
                        Name = "Kyungrae Kim",
                        Email = string.Empty,
                        Url = new Uri("http://linkedin.com/in/kyungrae-kim/"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
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

            RoleInitializer.SeedData(serviceProvider, userManager, Configuration, env);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Async Inn API V1");
                c.RoutePrefix = string.Empty;
            });

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
