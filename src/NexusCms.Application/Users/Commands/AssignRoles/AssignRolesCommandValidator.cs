using FluentValidation;

namespace NexusCms.Application.Users.Commands.AssignRoles;

public class AssignRolesCommandValidator : AbstractValidator<AssignRolesCommand>
{
    public AssignRolesCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleIds).NotNull();
    }
}
