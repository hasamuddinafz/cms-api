using System.Text.Json;
using System.Text.Json.Nodes;

namespace NexusCms.Application.Settings;

/// <summary>
/// Utility to mask/unmask the "password" field inside SmtpJson before sending
/// to the client or before persisting to the database.
/// </summary>
public static class SmtpJsonHelper
{
    public const string PasswordMask = "••••••";
    private const string PasswordKey = "password";

    /// <summary>
    /// Returns a copy of smtpJson with the password field replaced by PasswordMask.
    /// Used in GET responses so the real (encrypted) value is never sent to the browser.
    /// </summary>
    public static string MaskPassword(string smtpJson)
    {
        try
        {
            var node = JsonNode.Parse(smtpJson);
            if (node is JsonObject obj && obj.ContainsKey(PasswordKey))
                obj[PasswordKey] = PasswordMask;
            return node?.ToJsonString() ?? smtpJson;
        }
        catch { return smtpJson; }
    }

    /// <summary>
    /// When the incoming SmtpJson contains the mask value (user did not change the password),
    /// replace it with the previously stored encrypted value before saving.
    /// Returns the incoming json unchanged if a real new password is provided.
    /// </summary>
    public static string PreserveOrEncrypt(
        string incomingJson,
        string storedEncryptedJson,
        Func<string, string> encrypt)
    {
        try
        {
            var incoming = JsonNode.Parse(incomingJson) as JsonObject;
            if (incoming is null) return incomingJson;

            var rawPwd = incoming[PasswordKey]?.GetValue<string>();

            if (rawPwd == PasswordMask || string.IsNullOrWhiteSpace(rawPwd))
            {
                // Restore the already-stored encrypted value
                var stored = JsonNode.Parse(storedEncryptedJson) as JsonObject;
                incoming[PasswordKey] = stored?[PasswordKey]?.GetValue<string>() ?? string.Empty;
            }
            else
            {
                // Encrypt the new plaintext password
                incoming[PasswordKey] = encrypt(rawPwd);
            }

            return incoming.ToJsonString();
        }
        catch { return incomingJson; }
    }
}
