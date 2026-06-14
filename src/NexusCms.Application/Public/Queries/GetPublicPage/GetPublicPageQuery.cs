using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicPage;

/// <summary>Returns a single published page. Handler also triggers view-count increment.</summary>
public record GetPublicPageQuery(
    string ModelKey,
    string Slug) : IRequest<Result<PublicPageDto>>;
