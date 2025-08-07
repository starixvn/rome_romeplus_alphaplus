using AlphaPlus.IPInfo.Services;

namespace AlphaPlus.IPInfo.UnitTests.Services;

public class IPInfoHelperTests
{
    [Theory]
    [InlineData("192.168.1.1", 3232235777)]
    [InlineData("10.0.0.1", 167772161)]
    [InlineData("172.16.0.1", 2886729729)]
    [InlineData("127.0.0.1", 2130706433)]
    public void ConvertIPV4ToNumber_WithValidIPV4Address_ShouldReturnCorrectNumber(string ip, long expectedNumber)
    {
        // Act
        var result = IPInfoHelper.ConvertIPV4ToNumber(ip);

        // Assert
        Assert.Equal(expectedNumber, result);
    }

    [Theory]
    [InlineData("invalid.ip")]
    [InlineData("not.an.ip.address")]
    [InlineData("256.256.256.256")]
    [InlineData("")]
    [InlineData(null)]
    public void ConvertIPV4ToNumber_WithInvalidIP_ShouldReturnZero(string ip)
    {
        // Act
        var result = IPInfoHelper.ConvertIPV4ToNumber(ip);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void SplitIPV4AndIPV6List_WithValidIPV4Addresses_ShouldReturnCorrectSplit()
    {
        // Arrange
        var ipList = new List<string> { "192.168.1.1", "10.0.0.1", "172.16.0.1" };

        // Act
        var result = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Item1.Count); // IPv4 list
        Assert.Empty(result.Item2); // IPv6 list
        Assert.Contains("192.168.1.1", result.Item1);
        Assert.Contains("10.0.0.1", result.Item1);
        Assert.Contains("172.16.0.1", result.Item1);
    }

    [Fact]
    public void SplitIPV4AndIPV6List_WithValidIPV6Addresses_ShouldReturnCorrectSplit()
    {
        // Arrange
        var ipList = new List<string>
        {
            "2001:0db8:85a3:0000:0000:8a2e:0370:7334",
            "2001:db8::1",
            "::1"
        };

        // Act
        var result = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Item1); // IPv4 list
        Assert.Equal(3, result.Item2.Count); // IPv6 list
        Assert.Contains("2001:0db8:85a3:0000:0000:8a2e:0370:7334", result.Item2);
        Assert.Contains("2001:db8::1", result.Item2);
        Assert.Contains("::1", result.Item2);
    }

    [Fact]
    public void SplitIPV4AndIPV6List_WithMixedIPAddresses_ShouldReturnCorrectSplit()
    {
        // Arrange
        var ipList = new List<string>
        {
            "192.168.1.1",
            "2001:db8::1",
            "10.0.0.1",
            "::1"
        };

        // Act
        var result = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Item1.Count); // IPv4 list
        Assert.Equal(2, result.Item2.Count); // IPv6 list
        Assert.Contains("192.168.1.1", result.Item1);
        Assert.Contains("10.0.0.1", result.Item1);
        Assert.Contains("2001:db8::1", result.Item2);
        Assert.Contains("::1", result.Item2);
    }

    [Fact]
    public void SplitIPV4AndIPV6List_WithInvalidIPAddresses_ShouldReturnEmptyLists()
    {
        // Arrange
        var ipList = new List<string> { "invalid.ip", "not.an.ip.address", "256.256.256.256" };

        // Act
        var result = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Item1); // IPv4 list
        Assert.Empty(result.Item2); // IPv6 list
    }

    [Fact]
    public void SplitIPV4AndIPV6List_WithEmptyList_ShouldReturnEmptyLists()
    {
        // Arrange
        var ipList = new List<string>();

        // Act
        var result = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Item1); // IPv4 list
        Assert.Empty(result.Item2); // IPv6 list
    }

    [Fact]
    public void SplitIPV4AndIPV6List_WithMixedValidAndInvalidIPs_ShouldReturnOnlyValidIPs()
    {
        // Arrange
        var ipList = new List<string>
        {
            "192.168.1.1",
            "invalid.ip",
            "2001:db8::1",
            "not.an.ip.address"
        };

        // Act
        var result = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Item1); // IPv4 list
        Assert.Single(result.Item2); // IPv6 list
        Assert.Contains("192.168.1.1", result.Item1);
        Assert.Contains("2001:db8::1", result.Item2);
    }
}