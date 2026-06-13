using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Queries.GetLanguageList;

public record GetLanguageListQuery(bool ActiveOnly = false) : IRequest<Result<List<LanguageDto>>>;
