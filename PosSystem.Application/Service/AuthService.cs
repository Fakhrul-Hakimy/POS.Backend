using PosSystem.Application.Interfaces;
using PosSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace PosSystem.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAppDbContext _context;

        public AuthService(IConfiguration configuration , IAppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public Task<object?> LoginAsync(string email, string password){
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult<object?>(new { Success = false, Message = "Email and password are required." });
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash
                ))
            {
                return Task.FromResult<object?>(new { Success = false, Message = "Invalid email or password." });
            }

            var token = GenerateJwtToken(user);
            return Task.FromResult<object?>(new { Success = true, Token = token });
        }
        public async Task<object?> RegisterAsync(string name, string email, string password){
             // check if user with the same email already exists
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return new { Success = false, Message = "Email already in use." };
            }

            // create new user
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User" // default role
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new { Success = true, Message = "User registered successfully." };

        }

       private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
        };

        var tokenValue = _configuration.GetValue<string>("AppSettings:Token");
        if (string.IsNullOrWhiteSpace(tokenValue))
        {
            throw new InvalidOperationException("AppSettings:Token is missing.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration.GetValue<string>("AppSettings:Issuer"),
            Audience = _configuration.GetValue<string>("AppSettings:Audience"),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    }

 
}