using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HelperClasses
{
    public class PasswordHelper
    {
        public static string HashPassword(string password, string base64Salt = null, int iterations = 10_000)
        {
            if (string.IsNullOrWhiteSpace(base64Salt))
            {
                base64Salt = CreateRandomSalt();
            }

            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(base64Salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: iterations,
                                numBytesRequested: 512 / 8);

            var base64Hash = Convert.ToBase64String(valueBytes);

            StringBuilder sb = new StringBuilder();
            sb.Append(base64Salt);
            sb.Append("|");
            sb.Append(iterations.ToString());
            sb.Append("|");
            sb.Append(base64Hash);

            return sb.ToString();
        }

        public static bool ValidatePassword(string submittedPassword, string hash)
        {
            var passHashData = hash.Split('|');

            //The password data should contain a salt, iteration count, and password hash.
            if (passHashData.Length != 3)
            {
                return false;
            }

            //The validation fails if the iteration count cannot be parsed.
            if (!int.TryParse(passHashData[1], out int iterations))
            {
                return false;
            }

            return HashPassword(submittedPassword, passHashData[0], iterations) == hash;
        }

        private static string CreateRandomSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);

                var base64Salt = Convert.ToBase64String(randomBytes);
                return base64Salt;
            }
        }
    }
}
