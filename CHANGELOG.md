# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] - 2026-05-18

Stability and packaging release: bug fixes, safer randomness, leaner NuGet dependencies, and open-source repository layout. No new generator features beyond what existed in 0.1.x.

### Added

- xUnit test project (`Odex.AspNetCore.Generators.OidGen.Tests`)
- GitHub Actions CI and Release workflows
- `ServiceCollectionExtensions` (preferred DI entry type)
- `OidOptionsValidator` with startup validation
- XML documentation on public API
- Source Link via `Microsoft.SourceLink.GitHub`
- Root-level README, LICENSE, CHANGELOG, CONTRIBUTING, and repository metadata
- Central package management (`Directory.Packages.props`)
- Argument validation for lengths, masks, prefixes, and suffixes

### Fixed

- **`AddOidGen` now registers `IOptions<OidOptions>`** when no configure delegate is supplied
- **Thread-safe randomness** — `RandomNumberGenerator` replaces static `System.Random`
- **`Formatted` respects configured placeholder** when the placeholder argument is omitted
- Conflicting `uppercaseOnly` and `lowercaseOnly` now throws `ArgumentException`

### Changed

- `IOidGen` registered as **singleton** (was scoped; implementation is stateless)
- `AddOidGen` returns `IServiceCollection` for fluent registration
- `Formatted` signature uses `char? placeholder = null` instead of `char placeholder = '#'`
- `OidOptions` and `OidGenerator` implementation are `sealed`
- NuGet package depends only on `Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.Options` (removed direct `Microsoft.Extensions.DependencyInjection` reference)
- Package metadata, tags, and project layout aligned with open-source standards

### Removed

- `ServiceCollectionExtension` — use `ServiceCollectionExtensions.AddOidGen` instead

## [0.1.2] and earlier

Published to NuGet; see package release notes and prior README content.

[0.2.0]: https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/compare/v0.1.2...v0.2.0
