using System.Security.Cryptography;
using System.Text;

namespace API.Utils;

public static class PasswordHelper
{
    public static (string hashedPassword, string salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var combinedBytes = Encoding.UTF8.GetBytes(password).Concat(saltBytes).ToArray();
        
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(combinedBytes);
        var hashedPassword = Convert.ToBase64String(hashedBytes);

        return (hashedPassword, Convert.ToBase64String(saltBytes));
    }

    public static bool IsPasswordValid(string enteredPassword, string storedPassword, string salt)
    {
        var enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);
        var saltBytes = Convert.FromBase64String(salt);
        var saltedPasswordBytes = new byte[enteredPasswordBytes.Length + saltBytes.Length];
        Array.Copy(enteredPasswordBytes, saltedPasswordBytes, enteredPasswordBytes.Length);
        Array.Copy(saltBytes, 0, saltedPasswordBytes, enteredPasswordBytes.Length, saltBytes.Length);

        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(saltedPasswordBytes);
        var enteredHash = Convert.ToBase64String(hashedBytes);
            
        return string.Equals(enteredHash, storedPassword);
    }
}
