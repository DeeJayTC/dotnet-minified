using Microsoft.EntityFrameworkCore;

namespace Smoower.Minified.Tests;

public class Thing
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Rank { get; set; }
}

public class TestDb(DbContextOptions<TestDb> options) : DbContext(options)
{
    public DbSet<Thing> Things => Set<Thing>();
}

public static class TestDbFactory
{
    public static TestDb Create(string name)
        => new(new DbContextOptionsBuilder<TestDb>().UseInMemoryDatabase(name).Options);
}
