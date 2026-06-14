using MediatR;
using NexusCms.Application.Common.Models;
using NexusCms.Application.MailTemplates;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Queries.GetMailTemplateList;

public record GetMailTemplateListQuery(string? Search = null, int Page = 1, int PageSize = 20)
    : IRequest<Result<PagedResult<MailTemplateListItemDto>>>;
