using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Dashboard.Queries.GetDashboardStats;

public record GetDashboardStatsQuery : IRequest<Result<DashboardStatsDto>>;
