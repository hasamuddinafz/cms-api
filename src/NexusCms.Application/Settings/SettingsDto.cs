namespace NexusCms.Application.Settings;

/// <summary>Full settings response — SmtpJson.password is masked as "••••••".</summary>
public record AppSettingDto(
    string GeneralJson,
    string SeoJson,
    string SmtpJson,        // password field replaced with "••••••"
    string LocalizationJson,
    string ThemeJson,
    DateTime UpdatedAt
);
