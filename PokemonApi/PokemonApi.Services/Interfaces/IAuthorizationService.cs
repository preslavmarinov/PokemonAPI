using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApi.Services.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<IdentityResult?> RegisterAsync(string email, string password);

        public Task<string?> LoginAsync(string email, string password);
    }
}
