using Odex.AspNetCore.Generators.OidGen.Options;

namespace Odex.AspNetCore.Generators.OidGen.Tests;

public sealed class OidOptionsValidatorTests
{
    private readonly OidOptionsValidator _validator = new();

    [Fact]
    public void Validate_default_options_succeeds()
    {
        var result = _validator.Validate(null, new OidOptions());
        Assert.False(result.Failed);
    }

    [Fact]
    public void Validate_empty_special_chars_fails()
    {
        var result = _validator.Validate(null, new OidOptions { SpecialChars = string.Empty });
        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_null_special_chars_fails()
    {
        var result = _validator.Validate(null, new OidOptions { SpecialChars = null! });
        Assert.True(result.Failed);
    }
}
