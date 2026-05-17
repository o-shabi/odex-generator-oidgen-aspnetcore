# Odex.AspNetCore.Generators.OidGen

[![NuGet](https://img.shields.io/nuget/v/Odex.AspNetCore.Generators.OidGen)](https://www.nuget.org/packages/Odex.AspNetCore.Generators.OidGen)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Odex.AspNetCore.Generators.OidGen)](https://www.nuget.org/packages/Odex.AspNetCore.Generators.OidGen)
[![CI](https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/actions/workflows/ci.yml/badge.svg)](https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/actions/workflows/ci.yml)
[![Release](https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/actions/workflows/release.yml/badge.svg)](https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/actions/workflows/release.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Lightweight **unique ID generation** for **ASP.NET Core**. Produce numeric, alphabetic, or mixed strings; apply prefixes, suffixes, and mask-based formats; configure special characters and register everything with a single DI extension.

---

## Table of contents

- [About](#about)
- [Installation](#installation)
- [Requirements](#requirements)
- [Configuration](#configuration)
- [Quick start](#quick-start)
- [Capabilities](#capabilities)
- [Mask format](#mask-format)
- [Examples](#examples)
- [API reference](#api-reference)
- [Testing](#testing)
- [Upgrading](#upgrading)
- [CI/CD](#cicd)
- [Contributing](#contributing)
- [License](#license)
- [Links](#links)

---

## About

**Odex.AspNetCore.Generators.OidGen** (O'shabi Unique ID Generator) exposes `IOidGen` for building human-friendly identifiers in web APIs and services. Generation uses cryptographically secure randomness (`RandomNumberGenerator`), and options are validated at startup when registered through `AddOidGen`.

---

## Installation

```bash
dotnet add package Odex.AspNetCore.Generators.OidGen
```

Pin a version for reproducible builds:

```bash
dotnet add package Odex.AspNetCore.Generators.OidGen --version 0.2.0
```

---

## Requirements

| Item | Version |
|------|---------|
| **Target framework** | net9.0 (.NET 9 and later) |
| **.NET SDK** | 9.x |

**NuGet dependencies:** `Microsoft.Extensions.DependencyInjection.Abstractions`, `Microsoft.Extensions.Options`.

---

## Configuration

Configure defaults when registering services:

```csharp
builder.Services.AddOidGen(options =>
{
    options.Placeholder = '#';
    options.SpecialChars = "-_";
});
```

| Setting | Default | Description |
|---------|---------|-------------|
| `Placeholder` | `#` | Character replaced in `Formatted` when no explicit placeholder is passed. |
| `SpecialChars` | `-_` | Extra symbols used when `useSpecialCharacters` is `true`. Must not be empty. |

---

## Quick start

**1. Register in `Program.cs`:**

```csharp
using Odex.AspNetCore.Generators.OidGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOidGen();
var app = builder.Build();
```

**2. Inject `IOidGen`:**

```csharp
using Odex.AspNetCore.Generators.OidGen.Interfaces;

public class OrdersController(IOidGen oidGen) : ControllerBase
{
    [HttpPost]
    public IActionResult Create()
    {
        var orderId = oidGen.WithPrefix(12, "ORD-", useSpecialCharacters: false);
        return Ok(new { OrderId = orderId });
    }
}
```

---

## Capabilities

| Area | Summary |
|------|---------|
| **Numeric** | `Digit(length)` — digits only |
| **Alphabetic** | `Character(length, uppercaseOnly, lowercaseOnly)` |
| **Mixed** | `Mixed(length, useSpecialCharacters)` — letters and digits |
| **Prefix / suffix** | `WithPrefix`, `WithSuffix` |
| **Masks** | `Formatted(mask, placeholder?, useSpecialCharacters?)` |
| **DI** | `AddOidGen` registers singleton `IOidGen` with validated `OidOptions` |
| **Security** | `RandomNumberGenerator` for unbiased character selection |

---

## Mask format

`Formatted` walks the mask left to right. Each character equal to the effective placeholder is replaced with a random symbol from the mixed charset; all other characters are copied verbatim.

| Mask | Placeholder | Example output shape |
|------|-------------|----------------------|
| `PROD-###-####` | `#` (default) | `PROD-a3F-k9M2` |
| `TXN-***-***` | `*` | `TXN-x7K-9mP` |

When `placeholder` is omitted, `OidOptions.Placeholder` is used.

---

## Examples

### Numeric ID

```csharp
var numericId = oidGen.Digit(10);
// e.g. "8472910536"
```

### Uppercase alphabetic ID

```csharp
var upperId = oidGen.Character(8, uppercaseOnly: true);
// e.g. "KXMVRQPT"
```

### Mixed ID with special characters

```csharp
var mixedId = oidGen.Mixed(12, useSpecialCharacters: true);
// e.g. "aB3-xK9_mP2q"
```

### Prefixed order number

```csharp
var orderId = oidGen.WithPrefix(8, "ORD-", useSpecialCharacters: false);
// e.g. "ORD-a3Fk9M2x"
```

### Masked product code

```csharp
var productCode = oidGen.Formatted("PROD-###-####", useSpecialCharacters: false);
// e.g. "PROD-a3F-k9M2"
```

### Custom placeholder per call

```csharp
var txnId = oidGen.Formatted("TXN-***-***", placeholder: '*', useSpecialCharacters: false);
```

---

## API reference

### `IOidGen`

| Member | Description |
|--------|-------------|
| `Digit` | Random digit string of given length. |
| `Character` | Random letters; optional uppercase-only or lowercase-only. |
| `Mixed` | Random alphanumeric string; optional special characters. |
| `WithPrefix` | `prefix` + mixed segment. |
| `WithSuffix` | Mixed segment + `suffix`. |
| `Formatted` | Apply mask with placeholder substitution. |

### `ServiceCollectionExtensions`

| Method | Description |
|--------|-------------|
| `AddOidGen(IServiceCollection, Action<OidOptions>?)` | Registers `IOidGen`, `OidOptions`, and startup validation. |

XML documentation is included in the NuGet package for IntelliSense.

---

## Testing

Unit tests live in **Odex.AspNetCore.Generators.OidGen.Tests**. Run:

```bash
dotnet test
```

---

## Upgrading

See the [changelog](CHANGELOG.md) for version history and breaking changes.

**0.1.x → 0.2.0 (summary):**

- `AddOidGen` registers `IOptions<OidOptions>` and validates options on startup.
- `IOidGen` is registered as a **singleton** (was scoped).
- `Formatted` uses `char? placeholder` — omit to use `OidOptions.Placeholder`.
- Thread-safe randomness via `RandomNumberGenerator` (replaces `System.Random`).
- Argument validation for length, mask, prefix, and suffix.
- Use `ServiceCollectionExtensions.AddOidGen` (replaces `ServiceCollectionExtension`).

---

## CI/CD

| Workflow | File | Purpose |
|----------|------|---------|
| **CI** | [.github/workflows/ci.yml](.github/workflows/ci.yml) | Build and test on push/PR to `main`. |
| **Release** | [.github/workflows/release.yml](.github/workflows/release.yml) | Pack and publish to NuGet.org on version tags `v*.*.*`. |

---

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for local setup, pull-request guidelines, and the release process.

---

## License

This project is released under the MIT License.

Copyright (c) Asen O'Shabi.

---

## Links

| Resource | URL |
|----------|-----|
| NuGet Gallery | https://www.nuget.org/packages/Odex.AspNetCore.Generators.OidGen |
| Changelog | https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/blob/main/CHANGELOG.md |
| Source / issues | https://github.com/o-shabi/odex-generator-oidgen-aspnetcore |
