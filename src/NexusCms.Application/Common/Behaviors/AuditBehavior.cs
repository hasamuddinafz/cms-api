using MediatR;
using Microsoft.Extensions.Logging;

namespace NexusCms.Application.Common.Behaviors;

public class AuditBehavior<TRequest, TResponse>(ILogger<AuditBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        if (requestName.EndsWith("Command", StringComparison.OrdinalIgnoreCase))
            logger.LogInformation("Executing command: {Command} {@Payload}", requestName, request);

        var response = await next();
        return response;
    }
}
