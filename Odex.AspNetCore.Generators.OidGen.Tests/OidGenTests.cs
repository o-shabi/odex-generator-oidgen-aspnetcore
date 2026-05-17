using Odex.AspNetCore.Generators.OidGen.Interfaces;
using Odex.AspNetCore.Generators.OidGen.Options;
using Odex.AspNetCore.Generators.OidGen.Services;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace Odex.AspNetCore.Generators.OidGen.Tests;

public sealed class OidGenTests
{
    private static IOidGen CreateSut(Action<OidOptions>? configure = null)
    {
        var options = new OidOptions();
        configure?.Invoke(options);
        return new OidGenerator(MsOptions.Create(options));
    }

    [Fact]
    public void Digit_returns_requested_length()
    {
        var sut = CreateSut();
        var value = sut.Digit(10);
        Assert.Equal(10, value.Length);
    }

    [Fact]
    public void Digit_uses_only_digits()
    {
        var sut = CreateSut();
        var value = sut.Digit(32);
        Assert.All(value, c => Assert.True(char.IsDigit(c)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Digit_invalid_length_throws(int length)
    {
        var sut = CreateSut();
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.Digit(length));
    }

    [Fact]
    public void Character_uppercase_only_uses_uppercase_letters()
    {
        var sut = CreateSut();
        var value = sut.Character(24, uppercaseOnly: true);
        Assert.All(value, c => Assert.True(char.IsUpper(c)));
    }

    [Fact]
    public void Character_lowercase_only_uses_lowercase_letters()
    {
        var sut = CreateSut();
        var value = sut.Character(24, lowercaseOnly: true);
        Assert.All(value, c => Assert.True(char.IsLower(c)));
    }

    [Fact]
    public void Character_conflicting_case_flags_throws()
    {
        var sut = CreateSut();
        Assert.Throws<ArgumentException>(() => sut.Character(8, uppercaseOnly: true, lowercaseOnly: true));
    }

    [Fact]
    public void Mixed_without_specials_uses_alphanumeric_only()
    {
        var sut = CreateSut();
        var value = sut.Mixed(32, useSpecialCharacters: false);
        Assert.All(value, c => Assert.True(char.IsLetterOrDigit(c)));
    }

    [Fact]
    public void Mixed_with_specials_includes_configured_special_characters()
    {
        var sut = CreateSut(o => o.SpecialChars = "!@");
        var values = Enumerable.Range(0, 50).Select(_ => sut.Mixed(40, useSpecialCharacters: true)).ToList();
        Assert.Contains(values, v => v.Any(c => c is '!' or '@'));
    }

    [Fact]
    public void WithPrefix_prepends_literal_prefix()
    {
        var sut = CreateSut();
        var value = sut.WithPrefix(6, "ORD-", useSpecialCharacters: false);
        Assert.StartsWith("ORD-", value, StringComparison.Ordinal);
        Assert.Equal(10, value.Length);
    }

    [Fact]
    public void WithPrefix_null_prefix_throws()
    {
        var sut = CreateSut();
        Assert.Throws<ArgumentNullException>(() => sut.WithPrefix(6, null!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void WithPrefix_invalid_length_throws(int length)
    {
        var sut = CreateSut();
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.WithPrefix(length, "ORD-"));
    }

    [Fact]
    public void WithSuffix_appends_literal_suffix()
    {
        var sut = CreateSut();
        var value = sut.WithSuffix(6, "-2026", useSpecialCharacters: false);
        Assert.EndsWith("-2026", value, StringComparison.Ordinal);
        Assert.Equal(11, value.Length);
    }

    [Fact]
    public void Formatted_replaces_only_placeholders()
    {
        var sut = CreateSut(o => o.Placeholder = '#');
        var value = sut.Formatted("AB-##-CD", useSpecialCharacters: false);
        Assert.Equal(8, value.Length);
        Assert.Equal('A', value[0]);
        Assert.Equal('B', value[1]);
        Assert.Equal('-', value[2]);
        Assert.Equal('-', value[5]);
        Assert.Equal('C', value[6]);
        Assert.Equal('D', value[7]);
        Assert.True(char.IsLetterOrDigit(value[3]));
        Assert.True(char.IsLetterOrDigit(value[4]));
    }

    [Fact]
    public void Formatted_uses_options_placeholder_when_argument_omitted()
    {
        var sut = CreateSut(o => o.Placeholder = '*');
        var value = sut.Formatted("X**Y", useSpecialCharacters: false);
        Assert.Equal('X', value[0]);
        Assert.Equal('Y', value[3]);
        Assert.True(char.IsLetterOrDigit(value[1]));
        Assert.True(char.IsLetterOrDigit(value[2]));
    }

    [Fact]
    public void Formatted_explicit_placeholder_overrides_options()
    {
        var sut = CreateSut(o => o.Placeholder = '#');
        var value = sut.Formatted("A#B", placeholder: '#', useSpecialCharacters: false);
        Assert.Equal('A', value[0]);
        Assert.Equal('B', value[2]);
        Assert.True(char.IsLetterOrDigit(value[1]));
    }

    [Fact]
    public void Formatted_empty_mask_throws()
    {
        var sut = CreateSut();
        Assert.Throws<ArgumentException>(() => sut.Formatted(string.Empty));
    }

    [Fact]
    public void Formatted_null_mask_throws()
    {
        var sut = CreateSut();
        Assert.Throws<ArgumentNullException>(() => sut.Formatted(null!));
    }
}
