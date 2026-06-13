using MediatR;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.PageInstances.Commands.ChangePageInstanceStatus;

/// <summary>
/// Handles both Publish (Draft→Published) and Archive (Published→Archived) transitions.
/// </summary>
public record ChangePageInstanceStatusCommand(Guid Id, PageStatus TargetStatus) : IRequest<Result>;
