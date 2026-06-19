# CLAUDE.md — generation rules for Smoower.Minified

These rules are **strict**. When generating .NET code in this repository (or any
repo that references the `Smoower.Minified.*` packages), follow them exactly.

## Output

- Output **code only**. Do not explain unless explicitly asked.
- **No comments. No XML docs (`///`). No `#region`. No unnecessary blank lines.**
- **File-scoped namespaces**, **primary constructors**, **records** for DTOs.
- Nullable reference types + implicit usings on; latest C# language version.

## Imports (declare once per project, in GlobalUsings.cs)

```csharp
global using Smoower.Minified.Core;
global using Smoower.Minified.AspNetCore;
global using Smoower.Minified.EFCore;
global using Smoower.Minified.Hosting;
global using Smoower.Minified.Logging;
global using Ctl = Microsoft.AspNetCore.Mvc.ControllerBase;
global using Res = Microsoft.AspNetCore.Mvc.IActionResult;
global using AR  = Microsoft.AspNetCore.Mvc.ActionResult;
global using CT  = System.Threading.CancellationToken;
global using Cfg = Microsoft.Extensions.Configuration.IConfiguration;
global using Tr  = System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>;
```

## Use the compact syntax

- Attributes: `[API]`, `[RT(...)]`, `[HG]`, `[HPO]`, `[HPU]`, `[HPA]`, `[HD]`,
  `[AUTH]`, `[ANON]`, `[FB]`, `[FR]`, `[FQ]`, `[FH]`.
- Types: `Ctl` (base), `Tr` (async action return), `Res`/`AR` where they fit;
  use `ActionResult<T>` / `ILogger<T>` for generics (no open-generic aliases).
- EF query: `w`, `s`, `ob`, `obd`, `tb`, `tbd`, `sk`, `tk`, `nt`, `inc`, `lst`,
  `one`, `single`, `any`, `cnt`.
- EF write: `db.save()`, `db.Set.id(key)`, `db.add`/`db.upd`/`db.del`. Sync code
  uses the `S`-suffixed variants (`saveS`, `idS`, `addS`, `lstS`, `oneS`, ...);
  async is the default because that's where the token savings are.
- Result-fusing terminators (prefer these — they make actions single
  expressions): `q.ok1()` (200/404), `q.okl()` (200 list), `q.okc()` (200 count),
  `set.okId(key)` (200/404), `db.okAdd(e)` (200), `db.okNew(e)` (add+save, 201),
  `value.created()` (201), `db.delById<T>(key)` (204/404).
  Keep `async` only when the expression still contains an `await`.
- Swagger: `[P200]`/`[P201]`/`[P204]`/`[P400]`/`[P404]`/... and generic
  `[P200<UserDto>]` instead of `[ProducesResponseType(...)]`. Stack them.
- Guards: `nil()`, `emp()`, `none()`.
- HttpClient: `getJson<T>`, `postJson`, `putJson`, `patchJson`, `del`.
- Redis (`IDatabase`): `get`, `set`, `del`, `incr`, `getJson<T>`, `setJson<T>`.
- Logging (`ILogger`): `inf`, `wrn`, `err`, `dbg`.
- DI (`IServiceCollection`): `scoped`, `single`, `trans`.
- Validation: inherit `MiniValidator<T>` (not `AbstractValidator<T>`); use
  `req(x=>x.P)` / `rule(x=>x.P)` then chain `max`/`min`/`len`/`email`/`gt`/`lt`/`lte`/`rng`.
- JSON: `x.toJson()` / `s.fromJson<T>()` (System.Text.Json, or the Newtonsoft package).

## Forbidden tokens

Never emit these long-form tokens in generated code:

- `HttpGet`, `HttpPost`, `HttpPut`, `HttpPatch`, `HttpDelete`
- `Route(`, `ApiController`, `ControllerBase`
- `IActionResult`, `ActionResult` (non-generic — use `Res`/`Tr`; generics use `ActionResult<T>`)
- `.Where(`, `.Select(`, `.ToListAsync(`, `.FirstOrDefaultAsync(`, `.SaveChangesAsync(`
- `AddScoped(` / `AddSingleton(` / `AddTransient(` (use `scoped`/`single`/`trans`)
- XML comments (`///`), `#region` / `#endregion`

A coarse checker in `tests/Smoower.Minified.Tests/ForbiddenTokenCheckerTests.cs`
scans the sample for most of these.

## Never compact the contract

Keep route templates, HTTP verbs, status codes, and DTO property/JSON names
**stable**. The compact style changes how code is written, never its behavior.

## Reference shape

```csharp
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
```
