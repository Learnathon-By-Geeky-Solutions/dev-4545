using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Infrastructure.Services
{
    public static class PasswordHasher
    {
        public static string HashPassword(string ?password, string salt)
        {
            var saltedPassword = $"{salt}{password}";
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
}
