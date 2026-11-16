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

    public static class Contact
    {
        public const string View   = "contact.view";
        public const string Delete = "contact.delete";
        public const string Reply  = "contact.reply";
    }

    public static class Subscribers
    {
        public const string View   = "subscribers.view";
        public const string Delete = "subscribers.delete";
        public const string Export = "subscribers.export";
    }

    public static class Mail
    {
        public const string Send = "mail.send";
    }

    public static class MailTemplates
    {
        public const string View   = "mailtemplates.view";
        public const string Create = "mailtemplates.create";
        public const string Edit   = "mailtemplates.edit";
        public const string Delete = "mailtemplates.delete";
    }

    /// <summary>
    /// Returns all permission records for seeding. Preserves deterministic GUIDs
    /// so re-running seed is idempotent.
    /// </summary>
    public static IReadOnlyList<(Guid Id, string Key, string Module, string Label)> All() =>
    [
        // Pages
        (new Guid("10000001-0000-0000-0000-000000000001"), Pages.View,    "Pages", "View Pages"),
        (new Guid("10000001-0000-0000-0000-000000000002"), Pages.Create,  "Pages", "Create Pages"),
        (new Guid("10000001-0000-0000-0000-000000000003"), Pages.Edit,    "Pages", "Edit Pages"),
        (new Guid("10000001-0000-0000-0000-000000000004"), Pages.Delete,  "Pages", "Delete Pages"),
        (new Guid("10000001-0000-0000-0000-000000000005"), Pages.Publish, "Pages", "Publish Pages"),

        // PageModels
        (new Guid("10000002-0000-0000-0000-000000000001"), PageModels.View,   "PageModels", "View Page Models"),
        (new Guid("10000002-0000-0000-0000-000000000002"), PageModels.Create, "PageModels", "Create Page Models"),
        (new Guid("10000002-0000-0000-0000-000000000003"), PageModels.Edit,   "PageModels", "Edit Page Models"),
        (new Guid("10000002-0000-0000-0000-000000000004"), PageModels.Delete, "PageModels", "Delete Page Models"),

        // Media
        (new Guid("10000003-0000-0000-0000-000000000001"), Media.View,   "Media", "View Media"),
        (new Guid("10000003-0000-0000-0000-000000000002"), Media.Upload, "Media", "Upload Media"),
        (new Guid("10000003-0000-0000-0000-000000000003"), Media.Delete, "Media", "Delete Media"),

        // Menus
        (new Guid("10000004-0000-0000-0000-000000000001"), Menus.Manage, "Menus", "Manage Menus"),

        // Settings
        (new Guid("10000005-0000-0000-0000-000000000001"), Settings.GeneralEdit,     "Settings", "Edit General Settings"),
        (new Guid("10000005-0000-0000-0000-000000000002"), Settings.SeoEdit,         "Settings", "Edit SEO Settings"),
        (new Guid("10000005-0000-0000-0000-000000000003"), Settings.SmtpEdit,        "Settings", "Edit SMTP Settings"),
        (new Guid("10000005-0000-0000-0000-000000000004"), Settings.LocalizationEdit, "Settings", "Edit Localization Settings"),
        (new Guid("10000005-0000-0000-0000-000000000005"), Settings.LanguagesManage, "Settings", "Manage Languages"),
        (new Guid("10000005-0000-0000-0000-000000000006"), Settings.ThemeEdit,       "Settings", "Edit Theme Settings"),

        // Users
        (new Guid("10000006-0000-0000-0000-000000000001"), Users.Manage, "Users", "Manage Users"),

        // Roles
        (new Guid("10000007-0000-0000-0000-000000000001"), Roles.Manage, "Roles", "Manage Roles"),

        // Dashboard
        (new Guid("10000008-0000-0000-0000-000000000001"), Dashboard.View, "Dashboard", "View Dashboard"),

        // Activity
        (new Guid("10000009-0000-0000-0000-000000000001"), Activity.View, "Activity", "View Activity Log"),

        // Contact
        (new Guid("10000010-0000-0000-0000-000000000001"), Contact.View,   "Contact", "View Contact Messages"),
        (new Guid("10000010-0000-0000-0000-000000000002"), Contact.Delete, "Contact", "Delete Contact Messages"),
        (new Guid("10000010-0000-0000-0000-000000000003"), Contact.Reply,  "Contact", "Reply to Contact Messages"),

        // Subscribers
        (new Guid("10000011-0000-0000-0000-000000000001"), Subscribers.View,   "Subscribers", "View Subscribers"),
        (new Guid("10000011-0000-0000-0000-000000000002"), Subscribers.Delete, "Subscribers", "Delete Subscribers"),
        (new Guid("10000011-0000-0000-0000-000000000003"), Subscribers.Export, "Subscribers", "Export Subscribers"),

        // Mail
        (new Guid("10000012-0000-0000-0000-000000000001"), Mail.Send, "Mail", "Send Mail"),

        // MailTemplates
        (new Guid("10000013-0000-0000-0000-000000000001"), MailTemplates.View,   "MailTemplates", "View Mail Templates"),
        (new Guid("10000013-0000-0000-0000-000000000002"), MailTemplates.Create, "MailTemplates", "Create Mail Templates"),
        (new Guid("10000013-0000-0000-0000-000000000003"), MailTemplates.Edit,   "MailTemplates", "Edit Mail Templates"),
        (new Guid("10000013-0000-0000-0000-000000000004"), MailTemplates.Delete, "MailTemplates", "Delete Mail Templates"),
    ];
}
