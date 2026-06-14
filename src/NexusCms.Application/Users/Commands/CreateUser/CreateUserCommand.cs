using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FullName,
    string Email,
    string Password,
    bool   IsActive = true
) : IRequest<Result<Guid>>;
