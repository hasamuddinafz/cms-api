using FluentValidation;

namespace NexusCms.Application.Roles.Queries.GetRoleList;

public class GetRoleListQueryValidator : AbstractValidator<GetRoleListQuery>
{
    public GetRoleListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
