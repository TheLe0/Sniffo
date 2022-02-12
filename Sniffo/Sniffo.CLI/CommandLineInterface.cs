using System.Net;
using Sniffo.Core;

namespace Sniffo.CLI
{
    public class CommandLineInterface
    {
        private IpHandler ipHandler;

        public CommandLineInterface()
        {
            ipHandler = new IpHandler();
        }

        public void Start()
        {
            Console.WriteLine("Sniffio: A network packets Sniffer");
            Console.WriteLine("Made By: TheLe0 - Leonardo Tosin");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

            ipHandler.InitSocket(SelectIPAddress());

            if (ipHandler.Socket == null)
            {
                return;
            }

            Console.WriteLine("IP: " + ipHandler.IPAddress);

            if (ipHandler.Socket == null)
            {
                Console.WriteLine("Error: The socket was not initialized!");
                return;
            }

            do
            {
                Console.WriteLine("## [ {0:yyyy-MM-dd HH:mm:ss.fff} ] ##".PadLeft(80, '#'), DateTime.Now);

                Console.WriteLine(ipHandler.SniffedPackage());
            } while (Console.KeyAvailable == false || Console.ReadKey().Key != ConsoleKey.Escape);
        }

        private IPAddress SelectIPAddress()
        {
            Console.WriteLine("Computador: " + ipHandler.Hostname);

            var ipAddresses = ipHandler.GetAllAvaliableIp();

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
