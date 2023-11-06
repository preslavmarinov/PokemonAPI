using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthorizationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }
        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await this._userManager.FindByEmailAsync(email);
            bool checkPassword = await this._userManager.CheckPasswordAsync(user, password);

            if (user == null || !checkPassword)
            {
                return null;
            }

            var role = await this._userManager.GetRolesAsync(user);
            IdentityOptions options = new IdentityOptions();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.Id.ToString()),
                    new Claim("UserName", user.UserName!),
                    new Claim(options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault()!)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetSection("JwtSecret").Value)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public async Task<IdentityResult?> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return result;
            }
            await _userManager.AddToRoleAsync(user, "user");

            return result;
        }
    }
}
