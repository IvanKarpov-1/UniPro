using Carter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniPro.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter(configurator: configurator =>
            configurator.WithEmptyValidators());

        return services;
    }
}