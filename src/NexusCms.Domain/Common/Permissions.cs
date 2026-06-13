namespace NexusCms.Domain.Common;

/// <summary>
/// Canonical list of all permission keys in the system.
/// When adding a new feature, add its keys here; seed will automatically
/// propagate them to the database and grant them to SuperAdmin.
/// </summary>
public static class Permissions
{
    public static class Pages
    {
        public const string View    = "pages.view";
        public const string Create  = "pages.create";
        public const string Edit    = "pages.edit";
        public const string Delete  = "pages.delete";
        public const string Publish = "pages.publish";
    }

    public static class PageModels
    {
        public const string View   = "pagemodels.view";
        public const string Create = "pagemodels.create";
        public const string Edit   = "pagemodels.edit";
        public const string Delete = "pagemodels.delete";
    }

    public static class Media
    {
        public const string View   = "media.view";
        public const string Upload = "media.upload";
        public const string Delete = "media.delete";
    }

    public static class Menus
    {
        public const string Manage = "menus.manage";
    }

    public static class Settings
    {
        public const string GeneralEdit      = "settings.general.edit";
        public const string SeoEdit          = "settings.seo.edit";
        public const string SmtpEdit         = "settings.smtp.edit";
        public const string LocalizationEdit = "settings.localization.edit";
        public const string LanguagesManage  = "settings.languages.manage";
        public const string ThemeEdit        = "settings.theme.edit";
    }

    public static class Users
    {
        public const string Manage = "users.manage";
    }

    public static class Roles
    {
        public const string Manage = "roles.manage";
    }

    public static class Dashboard
    {
        public const string View = "dashboard.view";
    }

    public static class Activity
    {
        public const string View = "activity.view";
    }

    /// <summary>
    /// Returns all permission records for seeding. Preserves deterministic GUIDs
    /// so re-running seed is idempotent.
    /// </summary>
    public static IReadOnlyList<(Guid Id, string Key, string Module, string Label)> All() =>
    [
        // Pages
        (new Guid("10000001-0000-0000-0000-000000000001"), Pages.View,    "Pages", "Sayfaları Görüntüle"),
        (new Guid("10000001-0000-0000-0000-000000000002"), Pages.Create,  "Pages", "Sayfa Oluştur"),
        (new Guid("10000001-0000-0000-0000-000000000003"), Pages.Edit,    "Pages", "Sayfa Düzenle"),
        (new Guid("10000001-0000-0000-0000-000000000004"), Pages.Delete,  "Pages", "Sayfa Sil"),
        (new Guid("10000001-0000-0000-0000-000000000005"), Pages.Publish, "Pages", "Sayfa Yayınla"),

        // PageModels
        (new Guid("10000002-0000-0000-0000-000000000001"), PageModels.View,   "PageModels", "Sayfa Modellerini Görüntüle"),
        (new Guid("10000002-0000-0000-0000-000000000002"), PageModels.Create, "PageModels", "Sayfa Modeli Oluştur"),
        (new Guid("10000002-0000-0000-0000-000000000003"), PageModels.Edit,   "PageModels", "Sayfa Modeli Düzenle"),
        (new Guid("10000002-0000-0000-0000-000000000004"), PageModels.Delete, "PageModels", "Sayfa Modeli Sil"),

        // Media
        (new Guid("10000003-0000-0000-0000-000000000001"), Media.View,   "Media", "Medyayı Görüntüle"),
        (new Guid("10000003-0000-0000-0000-000000000002"), Media.Upload, "Media", "Medya Yükle"),
        (new Guid("10000003-0000-0000-0000-000000000003"), Media.Delete, "Media", "Medya Sil"),

        // Menus
        (new Guid("10000004-0000-0000-0000-000000000001"), Menus.Manage, "Menus", "Menüleri Yönet"),

        // Settings
        (new Guid("10000005-0000-0000-0000-000000000001"), Settings.GeneralEdit,     "Settings", "Genel Ayarları Düzenle"),
        (new Guid("10000005-0000-0000-0000-000000000002"), Settings.SeoEdit,         "Settings", "SEO Ayarlarını Düzenle"),
        (new Guid("10000005-0000-0000-0000-000000000003"), Settings.SmtpEdit,        "Settings", "SMTP Ayarlarını Düzenle"),
        (new Guid("10000005-0000-0000-0000-000000000004"), Settings.LocalizationEdit, "Settings", "Yerelleştirme Ayarlarını Düzenle"),
        (new Guid("10000005-0000-0000-0000-000000000005"), Settings.LanguagesManage, "Settings", "Dilleri Yönet"),
        (new Guid("10000005-0000-0000-0000-000000000006"), Settings.ThemeEdit,       "Settings", "Tema Ayarlarını Düzenle"),

        // Users
        (new Guid("10000006-0000-0000-0000-000000000001"), Users.Manage, "Users", "Kullanıcıları Yönet"),

        // Roles
        (new Guid("10000007-0000-0000-0000-000000000001"), Roles.Manage, "Roles", "Rolleri Yönet"),

        // Dashboard
        (new Guid("10000008-0000-0000-0000-000000000001"), Dashboard.View, "Dashboard", "Dashboard Görüntüle"),

        // Activity
        (new Guid("10000009-0000-0000-0000-000000000001"), Activity.View, "Activity", "Aktivite Geçmişini Görüntüle"),
    ];
}
