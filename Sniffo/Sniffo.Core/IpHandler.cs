using Sniffo.Domain;
using System.Net;
using System.Net.Sockets;

namespace Sniffo.Core
{
    public class IpHandler
    {
        public Socket? Socket { get; private set; }
        public IPAddress? IPAddress { get; private set; }
        public string Hostname { get; private set; }

        public IpHandler()
        {
            Hostname = Dns.GetHostName();
        }

        public void InitSocket(IPAddress ipAddress)
        {
            IPAddress = ipAddress;

            try
            {
                Socket = new Socket(
                    IPAddress.AddressFamily,
                    SocketType.Raw,
                    ProtocolType.IP);

                CreateSocket();
            } 
            catch (SocketException)
            {
                Console.WriteLine("Error: The socket could be not initialize. Verify your user permissions and if there's a firewall on the socket port");
            }
        }

        public IPAddress[] GetAllAvaliableIp()
        {
            var hostnameEntry = Dns.GetHostEntry(Hostname);
            var ipAddresses = hostnameEntry.AddressList;

            return ipAddresses;
        }

        public string SniffedPackage()
        {
            return IpV4Package();
        }

        private string IpV4Package()
        {
            if (Socket == null)
            {
                return "";
            }

            var buffer = new byte[ushort.MaxValue];
            var bufferLength = Socket.Receive(buffer);

            var protocolIPv4 = new IPv4(buffer, bufferLength);

            return protocolIPv4.ToString();
        }

        private void CreateSocket()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    CreateUnixSocket();
                break;
                case PlatformID.Win32NT:
                    CreateWindowsSocket();
                break;
                default:
                    throw new NotImplementedException("SocketType.Raw not mapped OS.");
            }
        }

        private void CreateWindowsSocket()
        {
            if (IPAddress == null || Socket == null)
            {
                return;
            }

            var ipEndPoint = new IPEndPoint(IPAddress, 0);
            Socket.Bind(ipEndPoint);

            if (IPAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                throw new NotImplementedException("SocketOptionName.HeaderIncluded works only for IPv4.");
            }

            Socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.HeaderIncluded, // `HeaderIncluded` works only for IPv4.
                true);

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new NotImplementedException("IOControlCode.ReceiveAll works only on Windows.");
            }

            var optionIn = new byte[] { 1, 0, 0, 0 };
            var optionOut = new byte[4];
            Socket.IOControl(
                IOControlCode.ReceiveAll, // `ReceiveAll` works only on Windows.
                optionIn,
                optionOut);
        }

        private void CreateUnixSocket()
        {
            throw new NotImplementedException("SocketType.Raw not works on Linux.");
        }
    }
}
