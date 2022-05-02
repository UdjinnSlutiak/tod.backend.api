using System;
using System.Security.Cryptography;
using Tod.Services.Abstractions;

namespace Tod.Services.Implementations
{
	public class Rfc2898PasswordHasher : IPasswordHasher
	{
        private const int ITERATIONS = 10_000;
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 20;

        public string GetHash(string password)
        {
            var salt = new byte[SALT_SIZE];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            var hash = pbkdf2.GetBytes(HASH_SIZE);

            var hashBytes = new byte[HASH_SIZE + SALT_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashBytes = Convert.FromBase64String(passwordHash);
            var salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            var hash = pbkdf2.GetBytes(HASH_SIZE);

            for (int i = 0; i < HASH_SIZE; i++)
            {
                if (hashBytes[i + SALT_SIZE] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

