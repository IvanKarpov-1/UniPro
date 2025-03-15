using Carter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniPro.Features.Configurations;
using UniPro.Features.Features.UniversityInfo.Universities;

namespace UniPro.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        
        services.AddCarter(configurator: configurator =>
            configurator.WithEmptyValidators());

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(typeof(AddUniversityEndpoint).Assembly);
        });
        
        MapsterConfigurations.Configure();

        return services;
    }
}