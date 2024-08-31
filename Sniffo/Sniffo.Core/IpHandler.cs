using System.Net;

namespace Sniffo.Core
{
    public abstract class IpHandler(IPAddress ipAddress)
    {
        public IPAddress IPAddress { get; private set; } = ipAddress;

        public abstract string SniffedPackage();

        protected abstract void CreateWindowsSocket();

        protected abstract void CreateUnixSocket();
    }
}
