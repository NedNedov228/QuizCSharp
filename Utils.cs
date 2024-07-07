using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Quiz
{

    internal class Utils
    {
        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }

        public static void DisplayMessages(ref string? error, ref string? success)
        {
            if (error != null)
            {
                // Red color
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                ResetMessages(ref error, ref success);
                Console.ResetColor();
            }
            if (success != null)
            {
                // Green color
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(success);
                ResetMessages(ref error, ref success);
                Console.ResetColor();
            }
        }

        public static void ResetMessages(ref string? error, ref string? success)
        {
            error = null;
            success = null;
        }

        public static bool isPasswordValid(string password)
        {
            if (password.Length < 8 && !(
                
                password.Contains('_')
                
             || password.Contains('-')
             || password.Contains('!')
             || password.Contains('@')
             || password.Contains('#')
             || password.Contains('$')
             || password.Contains('%')
             || password.Contains('^')
             || password.Contains('&')
             || password.Contains('*'))

             )

            {
                return false;
            }
            return true;
        }
        
}
}
