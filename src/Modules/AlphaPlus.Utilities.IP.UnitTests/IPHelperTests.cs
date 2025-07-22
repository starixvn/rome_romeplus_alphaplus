using System.Net;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Http;

public class IPHelperTests
{
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
    private readonly Mock<HttpContext> httpContextMock;
    private readonly Mock<HttpRequest> httpRequestMock;
    private readonly Mock<ConnectionInfo> connectionInfoMock;
    private readonly HeaderDictionary headers;
    private readonly IPHelper ipHelper;

    public IPHelperTests()
    {
        httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextMock = new Mock<HttpContext>();
        httpRequestMock = new Mock<HttpRequest>();
        connectionInfoMock = new Mock<ConnectionInfo>();
        headers = new HeaderDictionary();

        httpRequestMock.Setup(r => r.Headers).Returns(headers);
        httpContextMock.Setup(c => c.Request).Returns(httpRequestMock.Object);
        httpContextMock.Setup(c => c.Connection).Returns(connectionInfoMock.Object);
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        ipHelper = new IPHelperImpl(httpContextAccessorMock.Object);
    }

    [Theory]
    [InlineData("X-Actual-IP", "203.0.113.1", new[] { "203.0.113.1" })]
    [InlineData("X-Forwarded-For", "203.0.113.2", new[] { "203.0.113.2" })]
    [InlineData(null, "::1", new[] { "127.0.0.1" })]
    [InlineData(null, "203.0.113.3", new[] { "203.0.113.3" })]
    [InlineData("X-Actual-IP", "203.0.113.4, 203.0.113.5", new[] { "203.0.113.4", "203.0.113.5" })]
    public void GetClientIPs_ReturnsExpectedIPs(string headerKey, string ipValue, string[] expected)
    {
        headers.Clear();
        if (headerKey == "X-Actual-IP" && ipValue != null)
            headers["X-Actual-IP"] = ipValue;
        else if (headerKey == "X-Forwarded-For" && ipValue != null)
            headers["X-Forwarded-For"] = ipValue;

        if (headerKey == null)
        {
            connectionInfoMock.Setup(c => c.RemoteIpAddress).Returns(ipValue != null ? IPAddress.Parse(ipValue) : null);
        }
        else
        {
            connectionInfoMock.Setup(c => c.RemoteIpAddress).Returns(IPAddress.Parse("8.8.8.8"));
        }

        var result = ipHelper.GetClientIPs();
        Assert.Equal(expected.Select(e => e.Trim()), result.Select(r => r.Trim()));
    }

    [Fact]
    public void GetClientIP_ReturnsFirstIPTrimmed()
    {
        headers["X-Actual-IP"] = " 203.0.113.10 , 203.0.113.11 ";
        connectionInfoMock.Setup(c => c.RemoteIpAddress).Returns(IPAddress.Parse("8.8.8.8"));
        var result = ipHelper.GetClientIP();
        Assert.Equal("203.0.113.10", result);
    }

    [Theory]
    [InlineData("127.0.0.1", false)]
    [InlineData("::1", true)]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334", true)]
    [InlineData("192.168.1.1", false)]
    public void IsIPV6_ReturnsExpectedResult(string ip, bool expected)
    {
        var result = ipHelper.IsIPV6(ip);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("192.168.1.1", 3232235777UL)]
    [InlineData("0.0.0.0", 0UL)]
    [InlineData("255.255.255.255", 4294967295UL)]
    [InlineData("invalid", 0UL)]
    public void ConvertIPV4ToNumber_ReturnsExpectedResult(string ip, ulong expected)
    {
        var result = ipHelper.ConvertIPV4ToNumber(ip);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("::1", 1UL)]
    [InlineData("invalid", 0UL)]
    public void ConvertIPV6ToNumber_ReturnsExpectedResult(string ip, ulong expected)
    {
        var result = ipHelper.ConvertIPV6ToNumber(ip);
        Assert.Equal(expected, result);
    }
}