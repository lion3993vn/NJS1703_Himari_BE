using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Utils
{
    public class PasswordUtils
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA256);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, HashType.SHA256);
        }

        public static string GeneratePassword()
        {
            Random random = new Random();
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var password = new char[6];
            for (int i = 0; i < password.Length; i++)
            {
                int index = random.Next(allowedChars.Length);
                password[i] = allowedChars[index];
            }
            return new string(password);
        }
    }
}
