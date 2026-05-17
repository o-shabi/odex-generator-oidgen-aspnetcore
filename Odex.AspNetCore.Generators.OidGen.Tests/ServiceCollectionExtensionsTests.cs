using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Odex.AspNetCore.Generators.OidGen.Interfaces;
using Odex.AspNetCore.Generators.OidGen.Options;

namespace Odex.AspNetCore.Generators.OidGen.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddOidGen_registers_singleton_io_id_gen()
    {
        var services = new ServiceCollection();
        services.AddOidGen();

        var descriptor = Assert.Single(services, d => d.ServiceType == typeof(IOidGen));
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    [Fact]
    public void AddOidGen_configure_applies_options()
    {
        var services = new ServiceCollection();
        services.AddOidGen(o =>
        {
            o.Placeholder = '*';
            o.SpecialChars = "!";
        });

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<OidOptions>>().Value;
        Assert.Equal('*', options.Placeholder);
        Assert.Equal("!", options.SpecialChars);
    }

    [Fact]
    public void AddOidGen_resolves_io_id_gen()
    {
        var services = new ServiceCollection();
        services.AddOidGen();

        using var provider = services.BuildServiceProvider();
        var oidGen = provider.GetRequiredService<IOidGen>();
        Assert.NotEmpty(oidGen.Digit(8));
    }

    [Fact]
    public void AddOidGen_null_services_throws()
    {
        IServiceCollection? services = null;
        Assert.Throws<ArgumentNullException>(() => services!.AddOidGen());
    }

    [Fact]
    public void AddOidGen_invalid_special_chars_fails_validation_on_start()
    {
        var services = new ServiceCollection();
        services.AddOidGen(o => o.SpecialChars = string.Empty);

        using var provider = services.BuildServiceProvider();
        Assert.Throws<OptionsValidationException>(() => provider.GetRequiredService<IOidGen>());
    }
}
