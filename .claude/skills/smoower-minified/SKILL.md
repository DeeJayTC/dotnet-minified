---
name: smoower-minified
description: Generate ASP.NET Core / EF Core code using the Smoower.Minified compact syntax to cut output tokens (cost + generation time). Use whenever writing or editing .NET API controllers, EF Core queries, DI registration, logging, HttpClient, or Redis code in a project that references the Smoower.Minified.* packages.
---

# Smoower.Minified generation rules

Write the most compact valid C# using the `Smoower.Minified.*` helpers. The point
is fewer output tokens: lower API cost and faster generation. Output **code only**
unless asked to explain.

## Hard rules

- File-scoped namespaces, primary constructors, records for DTOs. Nullable on.
- No comments, no XML docs (`///`), no `#region`, no filler blank lines.
- Assume this `GlobalUsings.cs` exists (add it if missing):

```csharp
global using Smoower.Minified.Core;
global using Smoower.Minified.AspNetCore;
global using Smoower.Minified.EFCore;
global using Smoower.Minified.Hosting;
global using Smoower.Minified.Logging;
global using Ctl = Microsoft.AspNetCore.Mvc.ControllerBase;
global using Res = Microsoft.AspNetCore.Mvc.IActionResult;
global using Tr  = System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>;
global using CT  = System.Threading.CancellationToken;
global using Cfg = Microsoft.Extensions.Configuration.IConfiguration;
```

## Use these instead of the long forms

| Use | Not |
| --- | --- |
| `[API]` `[RT(...)]` `[HG]` `[HPO]` `[HPU]` `[HPA]` `[HD]` | `[ApiController]` `[Route]` `[HttpGet]` ... |
| `[AUTH]` `[ANON]` `[FB]` `[FR]` `[FQ]` `[FH]` | `[Authorize]` `[AllowAnonymous]` `[FromBody]` ... |
| `:Ctl` | `: ControllerBase` |
| `public Tr X(...)` | `public async Task<IActionResult> X(...)` |
| `.w(...)` `.s(...)` `.ob(...)` `.nt()` `.inc(...)` | `.Where` `.Select` `.OrderBy` `.AsNoTracking` `.Include` |
| `.lst()` `.one()` `.single()` `.any()` `.cnt()` | `.ToListAsync` `.FirstOrDefaultAsync` ... |
| `db.save()` `db.add(e)` `db.upd(e)` `db.del(e)` `db.Set.id(key)` | `SaveChangesAsync` `FindAsync` ... |
| `q.ok1()` `q.okl()` `q.okc()` `set.okId(k)` `db.okAdd(e)` `db.okNew(e)` `db.delById<T>(k)` | manual `await ...` + `Ok(...)`/`NotFound()`/`CreatedAtAction(...)` |
| `value.created()` | `CreatedAtAction(nameof(Get), new { id = x.Id }, x)` |
| `[P200]` `[P201]` `[P404]` `[P200<UserDto>]` | `[ProducesResponseType(StatusCodes.Status200OK)]` ... |
| `MiniValidator<T>` + `req`/`rule` + `max`/`email`/`gt`/`lte` | `AbstractValidator<T>` + `RuleFor(...).NotEmpty().MaximumLength(...)` |
| `x.toJson()` `s.fromJson<T>()` | `JsonSerializer.Serialize/Deserialize` (or `JsonConvert`) |
| `nil()` `emp()` `none()` | `string.IsNullOrWhiteSpace` ... |
| `log.inf(...)` `log.wrn(...)` `log.err(...)` | `LogInformation` `LogWarning` ... |
| `svc.scoped<I,T>()` `svc.single<T>()` `svc.trans<T>()` | `AddScoped` `AddSingleton` `AddTransient` |
| `c.getJson<T>(url)` `c.postJson(url,b)` | `GetFromJsonAsync` `PostAsJsonAsync` |
| `db.get(k)` `db.set(k,v)` `db.getJson<T>(k)` | StackExchange.Redis `StringGetAsync` ... |

Prefer the result-fusing terminators (`ok1`/`okl`/`okId`/`okAdd`/`delById`) so an
action is a single expression with no `async`/`await`/`return`/`Ok`/`NotFound`.
Keep `async` only when the expression still has an `await`.

Generics can't be aliased, so use `ILogger<T>` and `ActionResult<T>` directly.
Sync EF variants exist with an `S` suffix (`saveS`, `lstS`, `oneS`, ...) for code
that must be synchronous.

## Never compact the contract

Route templates, HTTP verbs, status codes, and DTO property/JSON names must stay
exactly as the API requires. Shorten the code, not the public contract.

## Target shape

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
