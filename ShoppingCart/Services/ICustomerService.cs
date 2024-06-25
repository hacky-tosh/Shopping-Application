using ShoppingCart.DTOs;

namespace ShoppingCart.Services
{
    public interface ICustomerService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
