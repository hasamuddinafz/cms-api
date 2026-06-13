namespace NexusCms.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    IReadOnlyList<string> Permissions { get; }
    bool HasPermission(string permissionKey);

    // Request context (used by PageViewTrackingBehavior)
    string? IpAddress { get; }
    string? UserAgent { get; }
    string? Referrer  { get; }
}
