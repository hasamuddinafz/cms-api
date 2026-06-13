using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Dashboard.Queries.GetTopPages;

public record GetTopPagesQuery(int Take = 10) : IRequest<Result<List<TopPageDto>>>;
