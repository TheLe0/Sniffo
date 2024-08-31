namespace Sniffo.Core
{
    using Sniffo.Domain;
    using System.Net;
    using System.Net.Sockets;

    public class Ipv4Handler : IpHandler
    {
        public Socket Socket { get; private set; }

        public Ipv4Handler(IPAddress ipAddress) : base(ipAddress)
        {
            Socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Raw,
                    ProtocolType.IP);

            CreateWindowsSocket();
        }

        public override string SniffedPackage()
        {
            var buffer = new byte[ushort.MaxValue];
            var bufferLength = Socket.Receive(buffer);

            var protocolIPv4 = new IPv4(buffer, bufferLength);

            return protocolIPv4.ToString();
        }

        protected override void CreateWindowsSocket()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new NotImplementedException("IOControlCode.ReceiveAll works only on Windows.");
            }

            var ipEndPoint = new IPEndPoint(IPAddress, 0);
            Socket.Bind(ipEndPoint);

            Socket.SetSocketOption(
                 SocketOptionLevel.IP,
                 SocketOptionName.HeaderIncluded, // `HeaderIncluded` works only for IPv4.
                 true);

            var optionIn = new byte[] { 1, 0, 0, 0 };
            var optionOut = new byte[4];
            Socket.IOControl(
                IOControlCode.ReceiveAll, // `ReceiveAll` works only on Windows.
                optionIn,
                optionOut);
        }

        protected override void CreateUnixSocket()
        {
            throw new NotImplementedException("SocketType.Raw not works on Linux.");
        }
    }
}
