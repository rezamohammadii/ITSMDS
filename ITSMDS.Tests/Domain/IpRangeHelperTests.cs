
using ITSMDS.Domain.Tools;
using System.Net;
using Xunit;


namespace ITSMDS.Tests.Domain;

public class IpRangeHelperTests
{
    [Theory]
    [InlineData("192.168.1.5", "192.168.1.0/24", true)]
    [InlineData("192.168.2.5", "192.168.1.0/24", false)]
    [InlineData("10.0.0.1", "10.0.0.0/8", true)]
    [InlineData("172.16.5.10", "172.16.0.0/16", true)]
    [InlineData("172.17.5.10", "172.16.0.0/16", false)]
    public void IsIpRange_ShouldReturnExpectedResult(string ipStr, string cidr, bool expected)
    {
        // Arrange
        IPAddress ip = IPAddress.Parse(ipStr);

        // Act
        bool result = IpRangeHelper.IsIpRange(ip, cidr);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("::ffff:192.168.1.1", "192.168.1.1")]
    [InlineData("::1", "127.0.0.1")]
    [InlineData("2001:db8::1", "2001:db8::1")] // non-mapped IPv6
    [InlineData("192.168.1.1", "192.168.1.1")] // IPv4 stays the same
    public void ToIPv4_ShouldReturnExpectedIPv4(string inputIp, string expectedIp)
    {
        // Arrange
        IPAddress ip = IPAddress.Parse(inputIp);
        IPAddress expected = IPAddress.Parse(expectedIp);

        // Act
        IPAddress result = IpRangeHelper.ToIPv4(ip);

        // Assert
        Assert.Equal(expected, result);
    }
}
