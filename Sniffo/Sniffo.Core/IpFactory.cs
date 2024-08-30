using System.Net;
using System.Net.Sockets;

namespace Sniffo.Core
{
    public static class IpFactory
    {
        public static IpHandler CreateIpHandler(IPAddress ipAddress)
        {
            try
            {
                return ipAddress.AddressFamily switch
                {
                    AddressFamily.InterNetwork => new Ipv4Handler(ipAddress),
                    AddressFamily.InterNetworkV6 => new Ipv6Handler(ipAddress),
                    _ => throw new NotImplementedException($"Address family {ipAddress.AddressFamily} not implemented.")
                };
            }
            catch (SocketException)
            {
                Console.WriteLine("Error: The socket could be not initialize. Verify your user permissions and if there's a firewall on the socket port");
                throw;
            }
        }
    }
}
