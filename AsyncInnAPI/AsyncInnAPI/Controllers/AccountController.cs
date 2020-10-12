using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AsyncInnAPI.Models;
using AsyncInnAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AsyncInnAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="register">A data transfer object containing registeration information</param>
        /// <returns>Response code with a message</returns>
        [Authorize(Policy = "PropertyManagerPrivilege")]
        [HttpPost, Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterAgent(RegisterDto register)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                UserName = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };

            if (User.IsInRole("PropertyManagerPrivilege") && (register.Role == "Property Manager" || register.Role == "District Manager"))
                return Unauthorized("Unauthorized Regiseration");

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, register.Role);

                return Ok("Account Creation Successful");
            }

            return BadRequest("Invalid Registeration");
        }

        /// <summary>
        /// Assigns a role to a user
        /// </summary>
        /// <param name="assignment">A data transfer object containing registeration information</param>
        /// <returns>Response code with a message</returns>
        [Authorize(Policy = "DistrictManagerPrivilege")]
        [HttpPost, Route("Assign/Role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDto assignment)
        {
            var user = await _userManager.FindByEmailAsync(assignment.Email);

            var result = await _userManager.AddToRoleAsync(user, assignment.Role);

            if (result.Succeeded)
                return Ok("Role Assignment Successful");

            return BadRequest("Invalid Role Assignment");
        }

        /// <summary>
        /// Logins a user
        /// </summary>
        /// <param name="login">A data transfer object containing login information</param>
        /// <returns>Response code with a JWT token</returns>
        [AllowAnonymous]
        [HttpPost, Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(login.Email);
                var identityRole = await _userManager.GetRolesAsync(user);
                var token = CreateToken(user, identityRole.ToList());

                return Ok(new
                {
                    jwt = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return BadRequest("Invalid Login Attempt");
        }

        private JwtSecurityToken CreateToken(ApplicationUser user, List<string> roles)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("UserId", user.Id),
            };

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var token = AuthenticateToken(authClaims);

            return token;
        }

        private JwtSecurityToken AuthenticateToken(List<Claim> claims)
        {
            string jwtKey = _webHostEnvironment.IsDevelopment()
                ? _configuration["JWT_KEY"]
                : Environment.GetEnvironmentVariable("JWT_KEY_ENV");

            string jwtIssuer = _webHostEnvironment.IsDevelopment()
                ? _configuration["JWT_ISSUER"]
                : Environment.GetEnvironmentVariable("JWT_ISSUER_ENV");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                expires: DateTime.UtcNow.AddHours(24),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}