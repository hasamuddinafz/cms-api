using MediatR;
using Microsoft.AspNetCore.Identity;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var exists = await userManager.FindByEmailAsync(request.Email);
        if (exists is not null)
            return Result.Failure<Guid>("Bu e-posta adresi zaten kullanılıyor.");

        var user = new ApplicationUser
        {
            UserName  = request.Email,
            Email     = request.Email,
            FullName  = request.FullName,
            IsActive  = request.IsActive,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<Guid>(errors);
        }

        return Result.Success(user.Id);
    }
}
