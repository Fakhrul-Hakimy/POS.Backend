using PosSystem.Application.Interfaces;

namespace PosSystem.Application.Service
{
    public class AuthService : IAuthService
    {
        public Task<object?> LoginAsync(string email, string password){
            return Task.FromResult<object?>(null);
        }
        public Task<object?> RegisterAsync(string name, string email, string password){
            return Task.FromResult<object?>(null);
        }
    }
}