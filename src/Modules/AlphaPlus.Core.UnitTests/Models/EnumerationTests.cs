namespace AlphaPlus.Core.UnitTests.Models;

public class EnumerationTests
{
    [Fact]
    public void FromValue_Should_Return_Static_Instance()
    {
        var level = Enumeration.FromValue<UserLevel>(2);
        Assert.Same(UserLevel.Agent, level);
    }

    [Fact]
    public void FromDisplayName_Should_Return_Static_Instance()
    {
        var level = Enumeration.FromDisplayName<UserLevel>("Master");
        Assert.Same(UserLevel.Master, level);
    }

    [Fact]
    public void GetAll_Should_Return_All_Static_Fields()
    {
        var all = Enumeration.GetAll<UserLevel>().ToList();
        Assert.Contains(UserLevel.Member, all);
        Assert.Contains(UserLevel.Agent, all);
        Assert.Contains(UserLevel.Master, all);
        Assert.Contains(UserLevel.Super, all);
    }

    [Fact]
    public void Comparison_Operators_Should_Work_By_Value()
    {
        Assert.True(UserLevel.Super > UserLevel.Master);
        Assert.True(UserLevel.Agent < UserLevel.Master);
        Assert.True(UserLevel.Agent <= UserLevel.Master);
        Assert.True(UserLevel.Master != UserLevel.Agent);
    }
}


