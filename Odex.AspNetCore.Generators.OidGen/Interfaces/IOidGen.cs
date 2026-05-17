namespace Odex.AspNetCore.Generators.OidGen.Interfaces;

/// <summary>
/// Generates random identifiers with digits, letters, masks, prefixes, and suffixes.
/// </summary>
public interface IOidGen
{
    /// <summary>Generates a numeric string of the given length.</summary>
    string Digit(int length);

    /// <summary>Generates an alphabetic string with optional case constraints.</summary>
    string Character(int length, bool uppercaseOnly = false, bool lowercaseOnly = false);

    /// <summary>Generates a mixed alphanumeric string, optionally including special characters.</summary>
    string Mixed(int length, bool useSpecialCharacters = false);

    /// <summary>Generates a mixed string and prepends <paramref name="prefix"/>.</summary>
    string WithPrefix(int length, string prefix, bool useSpecialCharacters = false);

    /// <summary>Generates a mixed string and appends <paramref name="suffix"/>.</summary>
    string WithSuffix(int length, string suffix, bool useSpecialCharacters = false);

    /// <summary>
    /// Fills placeholder characters in <paramref name="mask"/> with random symbols.
    /// When <paramref name="placeholder"/> is <see langword="null"/>, <see cref="Options.OidOptions.Placeholder"/> is used.
    /// </summary>
    string Formatted(string mask, char? placeholder = null, bool useSpecialCharacters = false);
}
