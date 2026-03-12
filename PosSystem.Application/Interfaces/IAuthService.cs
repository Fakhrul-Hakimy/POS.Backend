namespace PosSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<object?> LoginAsync(string email, string password);

        Task<object?> RegisterAsync(string name, string email, string password);


    }
}