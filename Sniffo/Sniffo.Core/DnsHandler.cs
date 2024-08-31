using System.Net;

namespace Sniffo.Core
{
    public class DnsHandler
    {
        public string Hostname { get; private set; }

        public DnsHandler()
        {
            Hostname = Dns.GetHostName();
        }

        public IPAddress[] GetAllAvaliableIp()
        {
            var hostnameEntry = Dns.GetHostEntry(Hostname);
            var ipAddresses = hostnameEntry.AddressList;

            return ipAddresses;
        }
    }
}
