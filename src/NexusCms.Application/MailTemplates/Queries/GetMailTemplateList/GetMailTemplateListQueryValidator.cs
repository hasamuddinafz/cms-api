using FluentValidation;

namespace NexusCms.Application.MailTemplates.Queries.GetMailTemplateList;

public class GetMailTemplateListQueryValidator : AbstractValidator<GetMailTemplateListQuery>
{
    public GetMailTemplateListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
