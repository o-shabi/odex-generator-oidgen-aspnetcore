using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Odex.AspNetCore.Generators.OidGen.Interfaces;
using Odex.AspNetCore.Generators.OidGen.Options;
using Odex.AspNetCore.Generators.OidGen.Services;

namespace Odex.AspNetCore.Generators.OidGen;

/// <summary>Dependency injection registration for OidGen.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IOidGen"/> as a singleton with optional <see cref="OidOptions"/> configuration.
    /// </summary>
    public static IServiceCollection AddOidGen(
        this IServiceCollection services,
        Action<OidOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddOptions<OidOptions>()
            .ValidateOnStart();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IValidateOptions<OidOptions>, OidOptionsValidator>());

        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }

        services.TryAddSingleton<IOidGen, OidGenerator>();
        return services;
    }
}
