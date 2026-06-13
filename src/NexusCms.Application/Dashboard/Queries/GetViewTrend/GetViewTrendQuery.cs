using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Dashboard.Queries.GetViewTrend;

public record GetViewTrendQuery(int Days = 30) : IRequest<Result<List<DailyViewDto>>>;
