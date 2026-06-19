using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;
using Smoower.Minified.EFCore;
using Xunit;

namespace Smoower.Minified.Tests;

public class CreatedResultTests
{
    [Fact]
    public void Created_Returns201WithBody()
    {
        var dto = new { Id = 1, Name = "x" };
        var result = Assert.IsType<ObjectResult>(dto.created());
        Assert.Equal(201, result.StatusCode);
        Assert.Same(dto, result.Value);
    }

    [Fact]
    public async Task OkNew_PersistsAndReturns201()
    {
        var db = TestDbFactory.Create(nameof(OkNew_PersistsAndReturns201));
        var result = Assert.IsType<ObjectResult>(await db.okNew(new Thing { Name = "x", Rank = 1 }));
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(1, await db.Things.cnt());
    }
}
