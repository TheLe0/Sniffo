using System.Net;
using Sniffo.Core;

namespace Sniffo.CLI
{
    public class CommandLineInterface
    {
        public void Start()
        {
            Console.WriteLine("Sniffio: A network packets Sniffer");
            Console.WriteLine("Made By: TheLe0 - Leonardo Tosin");
            Console.WriteLine("         aimperatori - Anderson Imperatori");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

            try
            {
                var ipHandler = IpFactory.CreateIpHandler(SelectIPAddress());

                Console.WriteLine("IP: " + ipHandler.IPAddress);

                do
                {
                    Console.WriteLine("## [ {0:yyyy-MM-dd HH:mm:ss.fff} ] ##".PadLeft(80, '#'), DateTime.Now);

                    Console.WriteLine(ipHandler.SniffedPackage());
                } while (Console.KeyAvailable == false || Console.ReadKey().Key != ConsoleKey.Escape);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private IPAddress SelectIPAddress()
        {
            var dnsHandler = new DnsHandler();

            Console.WriteLine("Computador: " + dnsHandler.Hostname);

            var ipAddresses = dnsHandler.GetAllAvaliableIp();

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
