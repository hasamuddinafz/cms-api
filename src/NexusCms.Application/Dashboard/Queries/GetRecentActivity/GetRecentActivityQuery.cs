using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Dashboard.Queries.GetRecentActivity;

public record GetRecentActivityQuery(int Take = 15) : IRequest<Result<List<RecentActivityDto>>>;
