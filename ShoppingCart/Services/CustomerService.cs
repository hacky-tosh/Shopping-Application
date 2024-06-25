using ShoppingCart.DTOs;
using ShoppingCart.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;



namespace ShoppingCart.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly CustomerContext _context;

        public CustomerService(CustomerContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == dto.Email))
            {
                return "Email already in use.";
            }

            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return "Registration successful.";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Email == dto.Email);
            if (customer == null || !VerifyPassword(dto.Password, customer.Password))
            {
                return "Invalid email or password.";
            }

            return "Login successful.";
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

    }
}
