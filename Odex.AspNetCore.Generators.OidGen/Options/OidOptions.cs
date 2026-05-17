namespace Odex.AspNetCore.Generators.OidGen.Options;

/// <summary>
/// Configuration for <see cref="Interfaces.IOidGen"/> mask placeholders and special characters.
/// </summary>
public sealed class OidOptions
{
    /// <summary>
    /// Placeholder character replaced in <see cref="Interfaces.IOidGen.Formatted"/> when no explicit placeholder is passed.
    /// </summary>
    public char Placeholder { get; set; } = '#';

    /// <summary>
    /// Characters appended to the mixed alphabet when special characters are enabled.
    /// </summary>
    public string SpecialChars { get; set; } = "-_";
}
