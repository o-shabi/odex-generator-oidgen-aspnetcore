# Contributing

Thank you for contributing to **Odex.AspNetCore.Generators.OidGen**.

## Development setup

```bash
git clone https://github.com/o-shabi/odex-generator-oidgen-aspnetcore.git
cd odex-generator-oidgen-aspnetcore
dotnet restore
dotnet build
dotnet test
```

The repository uses [central package management](Directory.Packages.props) and a root [nuget.config](nuget.config) that disables NuGet vulnerability audit for faster, more reliable restores.

## Pull requests

1. Fork the repository and branch from `main`.
2. Keep changes focused; include tests in **Odex.AspNetCore.Generators.OidGen.Tests** for behavior changes.
3. Ensure `dotnet build` and `dotnet test` pass.
4. Update [CHANGELOG.md](CHANGELOG.md) for user-visible changes.
5. Open a pull request with a clear description and test plan.

## Code style

- Follow existing naming and formatting in the library.
- Public APIs require XML documentation comments.
- Use file-scoped namespaces.

## Releasing

Releases are published to NuGet when a version tag is pushed (for example `v0.2.0`).

1. Update `Version` in `Odex.AspNetCore.Generators.OidGen.csproj` and [CHANGELOG.md](CHANGELOG.md).
2. Commit, push to `main`, then create and push a tag:

   ```bash
   git tag v0.2.0
   git push origin v0.2.0
   ```

3. The [Release workflow](.github/workflows/release.yml) builds, tests, packs, pushes to NuGet, and creates a GitHub Release.

### NuGet API key (repository secret)

Add a secret named `NUGET_API_KEY` in GitHub (**Settings → Secrets and variables → Actions**):

1. Create an API key at [nuget.org](https://www.nuget.org/account/apikeys) with **Push** scope for `Odex.AspNetCore.Generators.OidGen`.
2. Paste the key as the `NUGET_API_KEY` repository secret.

## Reporting issues

Use [GitHub Issues](https://github.com/o-shabi/odex-generator-oidgen-aspnetcore/issues) with:

- .NET version
- Package version
- Minimal reproduction steps or sample code
