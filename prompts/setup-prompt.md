# Smoower.Minified — setup / initialize prompt

Paste this to your AI assistant (Claude Code, Copilot, Cursor, ChatGPT with tool
access) in a **new or existing** .NET repo. It installs the right packages,
wires up `GlobalUsings.cs`, and teaches the assistant the compact style by
writing the rules into the repo's instruction file. It is safe to re-run; it
only adds what is missing.

---

You are setting up **Smoower.Minified** in this repository. Smoower.Minified is a
set of tiny C# libraries that shrink boilerplate-heavy .NET API code into short,
stable forms that are still 100% ordinary C# (no source generator, same IL,
fewer output tokens). Do the following steps in order. Output only a short
summary of what you changed at the end; do not explain each step as you go.

## 1. Find the projects

- If this is an **existing** repo, locate the application project(s) — the
  `.csproj` files. Prefer the one whose SDK is `Microsoft.NET.Sdk.Web`
  (an ASP.NET Core app). If there is no project yet (**new** repo), create a
  minimal ASP.NET Core Web API: `dotnet new webapi -controllers -o <Name>` and
  use that project.
- A class library or worker that only needs EF Core, Dapper, or logging helpers
  is fine too — pick packages by what the project actually does (step 2).

## 2. Install the packages

The packages multi-target `net8.0` / `net9.0` / `net10.0`. Install the **latest**
version of each: run the plain `dotnet add package` commands below, never pass
`--version` or pin a version.

**Default set for an ASP.NET Core backend** (use this when unsure):

```bash
dotnet add package Smoower.Minified.Core
dotnet add package Smoower.Minified.AspNetCore
dotnet add package Smoower.Minified.EFCore
dotnet add package Smoower.Minified.Hosting
dotnet add package Smoower.Minified.Logging
dotnet add package Smoower.Minified.Validation
```

**Pick by need** — only add what the project uses:

| Package | Add it when the project… |
| --- | --- |
| `Smoower.Minified.Core` | always (guards + base aliases, zero framework deps) |
| `Smoower.Minified.AspNetCore` | has controllers / returns `IActionResult` |
| `Smoower.Minified.MinimalApi` | uses Minimal APIs instead of controllers |
| `Smoower.Minified.EFCore` | queries or writes via EF Core |
| `Smoower.Minified.Hosting` | registers services in DI |
| `Smoower.Minified.Logging` | uses `ILogger` |
| `Smoower.Minified.Validation` | validates DTOs (FluentValidation under the hood) |
| `Smoower.Minified.Http` | calls other APIs with `HttpClient` |
| `Smoower.Minified.Redis` | uses StackExchange.Redis |
| `Smoower.Minified.Dapper` | queries via Dapper / `IDbConnection` |
| `Smoower.Minified.Json` | needs `toJson()` / `fromJson<T>()` (System.Text.Json) |
| `Smoower.Minified.Json.Newtonsoft` | same, but on Newtonsoft.Json |
| `Smoower.Minified.Identity` | uses ASP.NET Core Identity |
| `Smoower.Minified.Extensions` | wants the Clock / DateTime / env helpers |
| `Smoower.Minified.Testing` | the **test** project (xUnit assertion + attribute shorteners) |

Do **not** add `Json` and `Json.Newtonsoft` together; pick one.

## 3. Add GlobalUsings.cs

Aliases are not transitive across assemblies, so they must live in the consuming
project. Create or update `GlobalUsings.cs` in the project root. Include a
`global using` line for **each Smoower.Minified namespace whose package you
actually added** in step 2, plus the alias block. Do not import a namespace for a
package you did not install.

```csharp
global using Smoower.Minified.Core;
global using Smoower.Minified.AspNetCore;
global using Smoower.Minified.EFCore;
global using Smoower.Minified.Hosting;
global using Smoower.Minified.Logging;
global using Smoower.Minified.Validation;

global using Res = Microsoft.AspNetCore.Mvc.IActionResult;
global using AR  = Microsoft.AspNetCore.Mvc.ActionResult;
global using CT  = System.Threading.CancellationToken;
global using Cfg = Microsoft.Extensions.Configuration.IConfiguration;
global using Tr  = System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>;
```

Confirm the project (or `Directory.Build.props`) has `ImplicitUsings` enabled,
`Nullable` enabled, and `LangVersion` set to `latest`. Add them if missing.

## 4. Teach the assistant the rules

Pick the instruction file your AI tool reads and add the Smoower.Minified rules
to it (create the file if it does not exist; if it exists, append a clearly
delimited section and do not disturb unrelated content):

- **Claude Code** → `CLAUDE.md` (repo root). It may also read
  `.github/copilot-instructions.md` — if that already exists, update it too.
- **GitHub Copilot** → `.github/copilot-instructions.md`
- **Cursor** → `.cursor/rules/smoower.mdc`

Write this rules block into the chosen file:

```markdown
## Smoower.Minified — code generation rules

When generating ASP.NET Core / EF Core code, use the Smoower.Minified compact
helpers. These change how code is *written*, never what it *does*.

- **Before generating, ask which compaction level to apply**: L1 (aliases,
  readable, the default), L2 (short domain names with the contract pinned), or L3
  (whitespace-packed). Skip the question only if the user already chose. Keep the
  level for the rest of the session.
- Output code only (no comments, no XML docs `///`, no `#region`, no extra blank
  lines). File-scoped namespaces, primary constructors, records for DTOs,
  nullable on, latest C#.
- Assume `GlobalUsings.cs` imports the `Smoower.Minified.*` namespaces and
  declares the aliases `Res` (IActionResult), `AR` (ActionResult), `Tr`
  (Task<IActionResult>), `CT` (CancellationToken), `Cfg` (IConfiguration). `Ctl`
  is the controller base class from `Smoower.Minified.AspNetCore` (inherit it, not
  `ControllerBase`), not an alias. Add anything missing.
- Attributes: `[API]` `[RT("...")]` `[HG]` `[HPO]` `[HPU]` `[HPA]` `[HD]`
  `[AUTH]` `[ANON]` `[FB]` `[FR]` `[FQ]` `[FH]` — never `[ApiController]`,
  `[HttpGet]`, `[Route]`, `[FromBody]`, etc.
- Controller base `:Ctl`; async action return type `Tr`. Generics can't be
  aliased — use `ILogger<T>` / `ActionResult<T>` directly.
- EF query: `.w` `.s` `.ob` `.obd` `.tb` `.tbd` `.sk` `.tk` `.nt` `.inc` `.lst`
  `.one` `.single` `.any` `.cnt`. EF write: `db.save()` `db.add(e)` `db.upd(e)`
  `db.del(e)` `db.Set.id(key)`. Sync variants take an `S` suffix
  (`saveS`/`lstS`/`oneS`/…); async is the default.
- Result-fusing terminators (prefer — they make actions single expressions):
  `q.ok1()` (200/404), `q.okl()` (200 list), `q.okc()` (200 count),
  `set.okId(key)`, `db.okAdd(e)` (200), `db.okNew(e)` (add+save, 201),
  `value.created()` (201), `db.delById<T>(key)` (204/404). Keep `async` only when
  an `await` remains.
- Guards: `nil()` `emp()` `none()`. Logging: `log.inf/wrn/err/dbg(...)`.
  DI: `svc.scoped/single/trans<…>()`. Http: `c.getJson<T>(url)`
  `c.postJson(url, body)`. Redis: `db.get/set/getJson`.
- Swagger: `[P200]` `[P201]` `[P204]` `[P400]` `[P404]` `[P200<UserDto>]` instead
  of `[ProducesResponseType(...)]`; stack them.
- Validation: inherit `MiniValidator<T>` (not `AbstractValidator<T>`);
  `req(x=>x.P)`/`rule(x=>x.P)` then chain `max`/`min`/`len`/`email`/`gt`/`lt`/
  `lte`/`rng`. JSON: `x.toJson()` / `s.fromJson<T>()`. Dapper (`IDbConnection`):
  `q<T>`/`q1<T>`/`qs<T>`/`ex`/`scalar<T>`.
- Never compact the public contract: keep route templates, HTTP verbs, status
  codes, and DTO property/JSON names exactly as required.

Reference shape:

\```csharp
[API,RT("api/users")]
public class UsersController(AppDb db,ILogger<UsersController> log):Ctl{
 [HG("{id}")]public Tr Get(int id)=>db.Users.nt().w(x=>x.Id==id).s(x=>new{x.Id,x.Name,x.Email}).ok1();
 [HG]public Tr All()=>db.Users.nt().s(x=>new{x.Id,x.Name,x.Email}).okl();
 [HPO]public async Tr Post(UserIn r){
  if(r.Name.nil())return BadRequest();
  var x=await db.add(new User{Name=r.Name,Email=r.Email});
  log.inf("created {Id}",x.Id);
  return Ok(new{x.Id,x.Name,x.Email});
 }
 [HD("{id}")]public Tr Del(int id)=>db.delById<User>(id);
}
public record UserIn(string Name,string Email);
\```
```

(When you write the block above into the file, drop the `\` before each fenced
` ``` ` — the backslashes are only here to nest the example inside this prompt.)

## 5. Verify

Run `dotnet build` on the affected project and confirm it compiles. If you added
EF Core but the project has no `DbContext`, that's expected — the helpers light
up once a context exists. Report the package list you installed, the files you
created or edited, and the build result.
