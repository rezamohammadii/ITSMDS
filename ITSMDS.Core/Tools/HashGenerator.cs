
using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;

namespace ITSMDS.Core.Tools;

public class HashGenerator
{
    private const int SALT_SIZE = 16;
    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[SALT_SIZE];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Create the Argon2Config configuration object
        var config = new Argon2Config
        {
            Password = Encoding.UTF8.GetBytes(password),
            Salt = salt,
            TimeCost = 4,            // Number of iterations
            MemoryCost = 65536,       // Memory cost in KB (64 MB)
            Lanes = 2,                // Parallelism factor (number of threads)
            Threads = Environment.ProcessorCount, // Matches processor threads
            HashLength = 64           // Length of the resulting hash in bytes
        };

        // Create the Argon2 hasher with the specified config
        using var argon2 = new Argon2(config);
        SecureArray<byte> hashBytes = argon2.Hash();  // This returns a SecureArray<byte>

        // Convert the hash bytes to Base64 string
        string hash = Convert.ToBase64String(hashBytes.Buffer);

        // Convert the salt to Base64 to store with the hash
        string saltString = Convert.ToBase64String(salt);

        // Store salt and hash together, separated by a colon
        return $"{saltString}:{hash}";
    }

    public static bool IsHashPasswordValid(string storedSaltedHash, string inputPassword)
    {
        // Split the stored string to get the salt and hash
        var parts = storedSaltedHash.Split(':');
        if (parts.Length != 2)
        {
            return false; // Invalid format
        }

        string saltString = parts[0];
        string storedHash = parts[1];

        // Decode the salt from Base64
        byte[] salt = Convert.FromBase64String(saltString);

        // Configure Argon2 for verification with the extracted salt and input password
        var config = new Argon2Config
        {
            Password = Encoding.UTF8.GetBytes(inputPassword),
            Salt = salt,
            TimeCost = 4,
            MemoryCost = 65536,
            Lanes = 2,
            Threads = Environment.ProcessorCount,
            HashLength = 64
        };

        // Hash the input password and compare it to the stored hash
        using var argon2 = new Argon2(config);
        SecureArray<byte> hashBytes = argon2.Hash();  // This returns a SecureArray<byte>

        // Convert the hash bytes to Base64 string
        string hash = Convert.ToBase64String(hashBytes.Buffer);

        return hash == storedHash;
    }
}
