using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NexusCms.Application.Common.Behaviors;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Services;
using NexusCms.Application.Public;
using NexusCms.Application.Public.Queries.GetPublicPage;
using NexusCms.Domain.Common;

namespace NexusCms.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Specific behavior — only fires for GetPublicPageQuery
            cfg.AddBehavior<
                IPipelineBehavior<GetPublicPageQuery, Result<PublicPageDto>>,
                PageViewTrackingBehavior>();
        });
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IExportService, ExportService>();

        return services;
    }
}
