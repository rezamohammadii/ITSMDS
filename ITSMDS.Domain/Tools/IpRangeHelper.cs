
using System.Net;

namespace ITSMDS.Domain.Tools;

public static class IpRangeHelper
{
    public static bool IsIpRange(IPAddress ip, string cidr)
    {
        string[] parts = cidr.Split("/");
        var baseIP = IPAddress.Parse(parts[0]);
        var prefixLength = int.Parse(parts[1]);

        byte[] ipBytes = ip.GetAddressBytes();
        byte[] baseBytes = baseIP.GetAddressBytes();

        if (ipBytes.Length != baseBytes.Length)
        {
            return false; // IPV4 vs IPV6

        }

        int byteCount = prefixLength / 8;
        int bitCount = prefixLength % 8;

        for(int i = 0; i < byteCount; i++)
        {
            if (ipBytes[i] != baseBytes[i])
            {
                return false;
            }
        }

        if (bitCount > 0)
        {
            int mask = (byte)~(255 >> bitCount);

            if ((ipBytes[byteCount] & mask) != (baseBytes[byteCount] & mask))
                return false;
        }
        return true;
    }

    public static IPAddress ToIPv4(IPAddress ip)
    {
        if (ip != null && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            if (ip.IsIPv4MappedToIPv6)
            {
                ip = ip.MapToIPv4();
            }
            else if (ip.Equals(IPAddress.IPv6Loopback)) //  ::1
            {
                ip = IPAddress.Loopback; // تبدیل به 127.0.0.1
            }
        }
        return ip;
    }
}
