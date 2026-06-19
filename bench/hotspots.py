#!/usr/bin/env python3
"""Where do the tokens still hide? Cost of common .NET constructs vs a compact form.

Run: pip install tiktoken; python bench/hotspots.py
o200k_base (GPT-4o) is a proxy for Claude's tokenizer - read the deltas, not absolutes.
"""
import tiktoken
enc = tiktoken.get_encoding("o200k_base")
def t(s): return len(enc.encode(s))

pairs = [
    # (label, long form, compact candidate)
    ("SaveChanges (sync)", "db.SaveChanges();", "db.saveS();"),
    ("Find (sync)", "db.Users.Find(id)", "db.Users.idS(id)"),
    ("ToList (sync)", ".ToList()", ".lstS()"),
    ("builder boilerplate", "var builder = WebApplication.CreateBuilder(args);", "var b = WebApplication.CreateBuilder(args);"),
    ("AddControllers", "builder.Services.AddControllers();", "b.Services.AddControllers();"),
    ("MapControllers", "app.MapControllers();", "app.MapControllers();"),
    ("MapGet minimal", 'app.MapGet("/u/{id}", (int id) => ...)', 'app.mg("/u/{id}", (int id) => ...)'),
    ("GetSection bind", 'builder.Configuration.GetSection("Db").Get<DbOpts>()', 'cfg.bind<DbOpts>("Db")'),
    ("ProducesResponseType", "[ProducesResponseType(StatusCodes.Status200OK)]", "[P200]"),
    ("JsonSerializer", "JsonSerializer.Serialize(x)", "x.toJson()"),
    ("JsonDeserialize", "JsonSerializer.Deserialize<T>(s)", "s.fromJson<T>()"),
    ("Created result", 'return CreatedAtAction(nameof(Get), new { id = x.Id }, x);', "return ok201(x);"),
    ("typeof check", "if (x is null) return NotFound();", "// folded into ok1()"),
    ("DataAnnotations", "[Required, StringLength(100)] public string Name", "[Req, Len(100)] public string Name"),
]

print(f"{'construct':24}{'long':>6}{'short':>7}{'save':>7}")
tot_long = tot_short = 0
for label, a, b in pairs:
    la, lb = t(a), t(b)
    tot_long += la
    tot_short += lb
    print(f"{label:24}{la:6}{lb:7}{la-lb:+7}")
print(f"{'TOTAL':24}{tot_long:6}{tot_short:7}{tot_long-tot_short:+7}")
