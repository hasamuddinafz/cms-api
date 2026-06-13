using NexusCms.Domain.Entities;

namespace NexusCms.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(ApplicationUser user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
}
