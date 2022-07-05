using Sniffo.Domain;
using System.Net;
using System.Net.Sockets;

namespace Sniffo.Core
{
    public class IpHandler
    {
        private const int IOC_VENDOR = 0x18000000;
        private const int IOC_IN = -2147483648; //0x80000000;
        private const int SIO_RCVALL = IOC_IN | IOC_VENDOR | 1;

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
                switch (IPAddress?.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        InitSocketIPv4();
                            break;
                    case AddressFamily.InterNetworkV6:
                        InitSocketIPv6();
                        break;
                    default:
                        throw new NotImplementedException();
                };
            } 
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error: The socket could be not initialize. Verify your user permissions and if there's a firewall on the socket port");
            }
        }

        public void InitSocketIPv4()
        {
            Socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Raw,
                    ProtocolType.IP);

            CreateWindowsSocketIPv4();
        }

        public void InitSocketIPv6()
        {
            Socket = new Socket(
                    AddressFamily.InterNetworkV6,
                    SocketType.Raw,
                    ProtocolType.IP);

            CreateWindowsSocketIPv6();
        }

        public IPAddress[] GetAllAvaliableIp()
        {
            var hostnameEntry = Dns.GetHostEntry(Hostname);
            var ipAddresses = hostnameEntry.AddressList;

            return ipAddresses;
        }

        public string SniffedPackage()
        {
            return IPAddress?.AddressFamily switch
            {
                AddressFamily.InterNetwork => IpV4Package(),
                AddressFamily.InterNetworkV6 => IpV6Package(),
                _ => throw new NotImplementedException()
            };
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

        private string IpV6Package()
        {
            if (Socket == null)
            {
                return "";
            }

            var buffer = new byte[ushort.MaxValue];
            var bufferLength = Socket.Receive(buffer);

            var protocolIPv6 = new IPv6(buffer, bufferLength);

            return protocolIPv6.ToString();
        }

        /*private void CreateSocket()
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
        }*/

        private void CreateWindowsSocketIPv4()
        {
            if (IPAddress == null || Socket == null)
            {
                return;
            }

            var ipEndPoint = new IPEndPoint(IPAddress, 0);
            Socket.Bind(ipEndPoint);

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

        private void CreateWindowsSocketIPv6()
        {
            if (IPAddress == null || Socket == null)
            {
                return;
            }

            var ipEndPoint = new IPEndPoint(IPAddress, 0);
            Socket.Bind(ipEndPoint);

            Socket.IOControl(SIO_RCVALL, BitConverter.GetBytes((int)1), null);
        }

        private void CreateUnixSocket()
        {
            throw new NotImplementedException("SocketType.Raw not works on Linux.");
        }
    }
}
