using Sniffo.Domain;
using System.Net;
using System.Net.Sockets;

namespace Sniffo.Core
{
    public class Runner
    {
        private Socket _socket;
        private IPAddress _ipAddress;

        public Runner()
        {
            _ipAddress = SelectIPAddress();
            _socket = CreateSocket(_ipAddress);

        }

        public void StartSniffer()
        {
            Console.WriteLine("Analisador de Pacote de Rede");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Console.WriteLine("IP: " + _ipAddress);

            do
            {
                var buffer = new byte[ushort.MaxValue];
                var bufferLength = _socket.Receive(buffer);

                Console.WriteLine("## [ {0:yyyy-MM-dd HH:mm:ss.fff} ] ##".PadLeft(80, '#'), DateTime.Now);

                var protocolIPv4 = new IPv4(buffer, bufferLength);
                Console.WriteLine(protocolIPv4);
            } while (Console.KeyAvailable == false || Console.ReadKey().Key != ConsoleKey.Escape);
        }

        private Socket CreateSocket(IPAddress ipAddress)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                throw new NotImplementedException("SocketType.Raw not works on Linux.");
            }

            var socket = new Socket(
                ipAddress.AddressFamily,
                SocketType.Raw,
                ProtocolType.IP);

            var ipEndPoint = new IPEndPoint(ipAddress, 0);
            socket.Bind(ipEndPoint);

            if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                throw new NotImplementedException("SocketOptionName.HeaderIncluded works only for IPv4.");
            }

            socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.HeaderIncluded, // `HeaderIncluded` works only for IPv4.
                true);

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new NotImplementedException("IOControlCode.ReceiveAll works only on Windows.");
            }

            var optionIn = new byte[] { 1, 0, 0, 0 };
            var optionOut = new byte[4];
            socket.IOControl(
                IOControlCode.ReceiveAll, // `ReceiveAll` works only on Windows.
                optionIn,
                optionOut);

            return socket;
        }

        private IPAddress SelectIPAddress()
        {
            var hostname = Dns.GetHostName();
            Console.WriteLine("Computador: " + hostname);

            var hostnameEntry = Dns.GetHostEntry(hostname);
            var ipAddresses = hostnameEntry.AddressList;

            Console.WriteLine("Endereços IP:");
            for (var i = 0; i < ipAddresses.Length; i++)
            {
                var ipAddress = ipAddresses[i];
                Console.WriteLine("  " + (i + 1) + ") " + ipAddress);
            }

            int selectedIndex;
            do
            {
                Console.Write("Selecione: ");
            } while (int.TryParse(Console.ReadLine(), out selectedIndex) == false ||
                     selectedIndex < 1 ||
                     selectedIndex > ipAddresses.Length);

            return ipAddresses[selectedIndex - 1];
        }
    }
}