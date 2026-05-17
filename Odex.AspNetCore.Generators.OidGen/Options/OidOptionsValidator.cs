using Microsoft.Extensions.Options;

namespace Odex.AspNetCore.Generators.OidGen.Options;

internal sealed class OidOptionsValidator : IValidateOptions<OidOptions>
{
    public ValidateOptionsResult Validate(string? name, OidOptions options)
    {
        if (options.SpecialChars is null)
        {
            return ValidateOptionsResult.Fail($"{nameof(OidOptions.SpecialChars)} cannot be null.");
        }

        if (options.SpecialChars.Length == 0)
        {
            return ValidateOptionsResult.Fail($"{nameof(OidOptions.SpecialChars)} cannot be empty.");
        }

        return ValidateOptionsResult.Success;
    }
}
