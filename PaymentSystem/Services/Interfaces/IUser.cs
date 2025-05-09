using PaymentSystem.DTOs;

namespace PaymentSystem.Services.Interfaces
{
    public interface IUser
    {
        Task<string?> LoginAsync(LoginDto dto);
        Task<string?> RegisterAsync(RegisterDto dto);
    }
}
