using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Odex.AspNetCore.Generators.OidGen.Interfaces;
using Odex.AspNetCore.Generators.OidGen.Options;

namespace Odex.AspNetCore.Generators.OidGen.Services;

/// <inheritdoc cref="IOidGen" />
public sealed class OidGenerator(IOptions<OidOptions> options) : IOidGen
{
    private const string Digits = "0123456789";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private readonly OidOptions _options = options.Value;

    /// <inheritdoc />
    public string Digit(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
        return Generate(length, Digits);
    }

    /// <inheritdoc />
    public string Character(int length, bool uppercaseOnly = false, bool lowercaseOnly = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        if (uppercaseOnly && lowercaseOnly)
        {
            throw new ArgumentException(
                "Cannot request both uppercase-only and lowercase-only character sets.",
                nameof(uppercaseOnly));
        }

        var charSet = uppercaseOnly ? Uppercase
            : lowercaseOnly ? Lowercase
            : Uppercase + Lowercase;

        return Generate(length, charSet);
    }

    /// <inheritdoc />
    public string Mixed(int length, bool useSpecialCharacters = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
        return Generate(length, BuildMixedCharSet(useSpecialCharacters));
    }

    /// <inheritdoc />
    public string WithPrefix(int length, string prefix, bool useSpecialCharacters = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
        ArgumentNullException.ThrowIfNull(prefix);
        return string.Concat(prefix, Mixed(length, useSpecialCharacters));
    }

    /// <inheritdoc />
    public string WithSuffix(int length, string suffix, bool useSpecialCharacters = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
        ArgumentNullException.ThrowIfNull(suffix);
        return string.Concat(Mixed(length, useSpecialCharacters), suffix);
    }

    /// <inheritdoc />
    public string Formatted(string mask, char? placeholder = null, bool useSpecialCharacters = false)
    {
        ArgumentException.ThrowIfNullOrEmpty(mask);

        var effectivePlaceholder = placeholder ?? _options.Placeholder;
        var charSet = BuildMixedCharSet(useSpecialCharacters);
        var result = new StringBuilder(mask.Length);

        foreach (var c in mask)
        {
            result.Append(c == effectivePlaceholder ? GetRandomChar(charSet) : c);
        }

        return result.ToString();
    }

    private string BuildMixedCharSet(bool useSpecialCharacters)
    {
        var chars = Digits + Lowercase + Uppercase;
        if (!useSpecialCharacters)
        {
            return chars;
        }

        if (string.IsNullOrEmpty(_options.SpecialChars))
        {
            throw new InvalidOperationException(
                "OidOptions.SpecialChars must be configured when useSpecialCharacters is true.");
        }

        return chars + _options.SpecialChars;
    }

    private static string Generate(int length, string charSet)
    {
        if (charSet.Length == 0)
        {
            throw new ArgumentException("Character set cannot be empty.", nameof(charSet));
        }

        var result = new char[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = GetRandomChar(charSet);
        }

        return new string(result);
    }

    private static char GetRandomChar(string charSet)
        => charSet[RandomNumberGenerator.GetInt32(charSet.Length)];
}
