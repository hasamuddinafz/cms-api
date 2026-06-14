using FluentValidation;

namespace NexusCms.Application.Public.Queries.GetPublicPageList;

public class GetPublicPageListQueryValidator : AbstractValidator<GetPublicPageListQuery>
{
    public GetPublicPageListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
