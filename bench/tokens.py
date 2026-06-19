#!/usr/bin/env python3
"""Token benchmark for Smoower.Minified.

Compares a hand-written conventional ASP.NET Core controller against the
Smoower.Minified sample controller (which folds in the former "Ultra" result
terminators). Both do the SAME work: CRUD + a structured log on create.

Uses tiktoken's o200k_base (the GPT-4o encoding) as a BPE proxy: it is NOT
Claude's tokenizer, so treat the absolute numbers as illustrative and the
*ratios* as the takeaway.

    pip install tiktoken
    python bench/tokens.py
"""
import os
import tiktoken

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
enc = tiktoken.get_encoding("o200k_base")


def toks(s: str) -> int:
    return len(enc.encode(s))


def read(rel: str) -> str:
    with open(os.path.join(ROOT, rel), encoding="utf-8") as f:
        return f.read()


# Conventional baseline: equivalent behavior to the sample controller.
VANILLA = r'''[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDb _db;
    private readonly ILogger<UsersController> _log;
    private readonly Clock _clock;
    public UsersController(AppDb db, ILogger<UsersController> log, Clock clock)
    {
        _db = db;
        _log = log;
        _clock = clock;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var x = await _db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new { u.Id, u.Name, u.Email })
            .FirstOrDefaultAsync();
        return x == null ? NotFound() : Ok(x);
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var x = await _db.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.Name, u.Email })
            .ToListAsync();
        return Ok(x);
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserIn r)
    {
        if (string.IsNullOrWhiteSpace(r.Name))
            return BadRequest();
        var x = new User { Name = r.Name, Email = r.Email };
        _db.Users.Add(x);
        await _db.SaveChangesAsync();
        _log.LogInformation("created user {Id} at {At:o}", x.Id, _clock.UtcNow);
        return Ok(new { x.Id, x.Name, x.Email });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Del(int id)
    {
        var x = await _db.Users.FindAsync(id);
        if (x == null)
            return NotFound();
        _db.Users.Remove(x);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
public record UserIn(string Name, string Email);'''

SMOOWER = read("samples/Smoower.Minified.SampleApi/Controllers/UsersController.cs")

base = toks(VANILLA)
print(f"{'variant':10} {'chars':>6} {'tokens':>7} {'vs vanilla':>11}")
for name, code in [("vanilla", VANILLA), ("smoower", SMOOWER)]:
    n = toks(code)
    pct = "" if name == "vanilla" else f"{(n - base) / base:+.0%}"
    print(f"{name:10} {len(code):6} {n:7} {pct:>11}")
