using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using SolarApp.Data;
using SolarApp.Models;
using SolarApp.Services; 


namespace SolarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

       
        private readonly SolarDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthController(SolarDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AuthController> logger, AuthService authService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
              _httpClientFactory = httpClientFactory;
            _authService= authService;


        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
                return BadRequest("Username already exists!");

            if (string.IsNullOrEmpty(user.Password))
                return BadRequest("Password is required!");

            user.Password = PasswordHasher.HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Registration is fine, you are ready to add Solar Plant.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var dbUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);

            // Check if the user exists and the password is correct
            if (dbUser == null || !PasswordHasher.VerifyPassword(user.Password, dbUser.Password))
                return Unauthorized(" Wrong login data!");

            var token = _authService.GenerateJwtToken(dbUser);
            return Ok(new { Token = token });
        }


    public static class PasswordHasher
    {
        private const int SaltSize = 16; 
        private const int HashSize = 20; 
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    var hash = pbkdf2.GetBytes(HashSize);
                    var hashBytes = new byte[SaltSize + HashSize];
                    Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
                    Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[SaltSize + i] != hash[i])
                        return false;
                }
            }
            return true;
        }
    }
    }
}




