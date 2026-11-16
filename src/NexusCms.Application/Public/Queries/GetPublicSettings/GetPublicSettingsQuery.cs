using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicSettings;

public record GetPublicSettingsQuery(string Lang = "en") : IRequest<Result<PublicSettingsDto>>;
