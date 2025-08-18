using Fanex.Alpha.Before.Extensions;
using Fanex.Alpha.Before.UnitTests.TestDoubles;
using Microsoft.AspNetCore.Http;

namespace Fanex.Alpha.Before.UnitTests.Extensions;

public class SessionExtensionTests
{
    [Fact]
    public void SetAndGet_RoundTripsComplexType()
    {
        ISession session = new TestSession();

        var payload = new Person { Name = "Alice", Age = 30 };
        session.Set("person", payload);

        var actual = session.Get<Person>("person");

        Assert.NotNull(actual);
        Assert.Equal("Alice", actual!.Name);
        Assert.Equal(30, actual.Age);
    }

    [Fact]
    public void Get_WhenMissing_ReturnsDefault()
    {
        ISession session = new TestSession();
        var actual = session.Get<Person>("missing");
        Assert.Null(actual);
    }

    private sealed record Person
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
    }
}


