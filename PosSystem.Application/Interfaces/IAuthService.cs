namespace PosSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);

        Task<object?> RegisterAsync(string name, string email, string password);


    }
}