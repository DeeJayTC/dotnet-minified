// Framework-agnostic base aliases. Using aliases are not transitive across
// assemblies, so consuming projects re-declare these in their own GlobalUsings.cs.
// See README.md for the full alias set and the open-generic limitation.

global using CT = System.Threading.CancellationToken;
global using Cfg = Microsoft.Extensions.Configuration.IConfiguration;
