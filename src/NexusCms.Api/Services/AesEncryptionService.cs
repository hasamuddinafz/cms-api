using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using NexusCms.Application.Common.Interfaces;

namespace NexusCms.Api.Services;

/// <summary>
/// AES-256-CBC encryption with a random IV prepended to the ciphertext.
/// Key is derived via SHA-256 from the "Encryption:Key" config value so any
/// string length is accepted; store the config value as a long random secret.
/// </summary>
public class AesEncryptionService(IConfiguration configuration) : IEncryptionService
{
    private readonly byte[] _key = DeriveKey(
        configuration["Encryption:Key"]
        ?? throw new InvalidOperationException("Encryption:Key is not configured."));

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key  = _key;
        aes.Mode = CipherMode.CBC;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var plainBytes  = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Prepend 16-byte IV so Decrypt can read it back
        var result = new byte[aes.IV.Length + cipherBytes.Length];
        aes.IV.CopyTo(result, 0);
        cipherBytes.CopyTo(result, aes.IV.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        var combined    = Convert.FromBase64String(cipherText);
        var iv          = combined[..16];
        var cipherBytes = combined[16..];

        using var aes = Aes.Create();
        aes.Key  = _key;
        aes.IV   = iv;
        aes.Mode = CipherMode.CBC;

        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    private static byte[] DeriveKey(string secret)
        => SHA256.HashData(Encoding.UTF8.GetBytes(secret));
}
