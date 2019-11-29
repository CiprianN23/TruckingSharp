using System;
using System.Security.Cryptography;

namespace TruckingSharp.Services
{
    public static class PasswordHashingService
    {
        private const int HashByteSize = 32;
        private const int Iterations = 10000;
        private const int SaltByteSize = 32;

        public static string GetPasswordHash(string password)
        {
            var salt = GetSalt();

            var saltBytes = Convert.FromBase64String(salt);
            byte[] derived;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA512))
            {
                derived = pbkdf2.GetBytes(HashByteSize);
            }

            return $"{Iterations}:{Convert.ToBase64String(derived)}:{Convert.ToBase64String(saltBytes)}";
        }

        public static bool VerifyPasswordHash(string password, string hash)
        {
            try
            {
                var parts = hash.Split(new[] { ':' });

                var saltBytes = Convert.FromBase64String(parts[2]);
                byte[] derived;

                var iterations = Convert.ToInt32(parts[0]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    saltBytes,
                    iterations,
                    HashAlgorithmName.SHA512))
                {
                    derived = pbkdf2.GetBytes(HashByteSize);
                }

                var newHash = $"{Iterations}:{Convert.ToBase64String(derived)}:{Convert.ToBase64String(saltBytes)}";

                return hash == newHash;
            }
            catch
            {
                return false;
            }
        }

        private static string GetSalt()
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            var bSalt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(bSalt);
            return Convert.ToBase64String(bSalt);
        }
    }
}