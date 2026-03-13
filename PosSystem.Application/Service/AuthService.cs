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


        public async Task<AuthResponse> LoginAsync(string email, string password){
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return (new AuthResponse { Success = false, Message = "Email and password are required." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash
                ))
            {
                return (new AuthResponse { Success = false, Message = "Invalid email or password." });
            }

            var token = GenerateJwtToken(user);

            user.lastLogin = DateTime.UtcNow+TimeSpan.FromHours(8); // Adjust for UTC+8 timezone
            await _context.SaveChangesAsync();


      
            return (new AuthResponse { Success = true, Token = token, Role = user.Role });
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