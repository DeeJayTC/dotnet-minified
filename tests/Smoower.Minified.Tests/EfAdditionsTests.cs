using Microsoft.EntityFrameworkCore;
using Smoower.Minified.EFCore;

namespace Smoower.Minified.Tests;

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Post> Posts { get; } = [];
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public List<Tag> Tags { get; } = [];
}

public class Tag
{
    public int Id { get; set; }
    public string Label { get; set; } = "";
}

public class BlogDb(DbContextOptions<BlogDb> options) : DbContext(options)
{
    public DbSet<Blog> Blogs => Set<Blog>();
}

public class EfAdditionsTests
{
    private static async Task<TestDb> Seed(string name)
    {
        var db = TestDbFactory.Create(name);
        db.Things.AddRange(
            new Thing { Name = "a", Rank = 1 },
            new Thing { Name = "b", Rank = 2 },
            new Thing { Name = "c", Rank = 3 });
        await db.SaveChangesAsync();
        return db;
    }

    [F]
    public async Task MaxMin_Aggregate()
    {
        var db = await Seed(nameof(MaxMin_Aggregate));
        (await db.Things.max(t => t.Rank)).eq(3);
        (await db.Things.min(t => t.Rank)).eq(1);
    }

    [F]
    public async Task Ntir_DoesNotTrack()
    {
        var name = nameof(Ntir_DoesNotTrack);
        (await Seed(name)).Dispose();
        var db = TestDbFactory.Create(name);
        (await db.Things.ntir().lst()).Count.eq(3);
        db.ChangeTracker.Entries().empty();
    }

    [F]
    public async Task Gb_GroupsRows()
    {
        var db = await Seed(nameof(Gb_GroupsRows));
        var groups = await db.Things.gb(t => t.Rank % 2 == 0).s(g => new { g.Key, Count = g.Count() }).lst();
        groups.Count.eq(2);
    }

    [F]
    public async Task Tinc_LoadsNestedGraph()
    {
        var db = new BlogDb(new DbContextOptionsBuilder<BlogDb>()
            .UseInMemoryDatabase(nameof(Tinc_LoadsNestedGraph)).Options);
        var blog = new Blog { Name = "b" };
        var post = new Post { Title = "p" };
        post.Tags.Add(new Tag { Label = "t" });
        blog.Posts.Add(post);
        db.Blogs.Add(blog);
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var loaded = await db.Blogs.inc(b => b.Posts).tinc(p => p.Tags).one();
        loaded!.Posts.Single().Tags.Single().Label.eq("t");
    }
}
