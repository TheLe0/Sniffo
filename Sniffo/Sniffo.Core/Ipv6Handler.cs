using Sniffo.Domain;
using System.Net;
using System.Net.Sockets;

namespace Sniffo.Core
{
    public class Ipv6Handler : IpHandler
    {
        public Socket Socket { get; private set; }

        private const int IOC_VENDOR = 0x18000000;
        private const int IOC_IN = -2147483648; //0x80000000;
        private const int SIO_RCVALL = IOC_IN | IOC_VENDOR | 1;

        public Ipv6Handler(IPAddress ipAddress) : base(ipAddress)
        {
            Socket = new Socket(
                    AddressFamily.InterNetworkV6,
                    SocketType.Raw,
                    ProtocolType.IP);

            CreateWindowsSocket();
        }

        public override string SniffedPackage()
        {
            var buffer = new byte[ushort.MaxValue];
            var bufferLength = Socket.Receive(buffer);

            var protocolIPv6 = new IPv6(buffer, bufferLength);

            return protocolIPv6.ToString();
        }

        protected override void CreateWindowsSocket()
        {
            var ipEndPoint = new IPEndPoint(IPAddress, 0);
            Socket.Bind(ipEndPoint);

            Socket.IOControl(SIO_RCVALL, BitConverter.GetBytes((int)1), null);
        }

        protected override void CreateUnixSocket()
        {
            throw new NotImplementedException("SocketType.Raw not works on Linux.");
        }
    }
}
