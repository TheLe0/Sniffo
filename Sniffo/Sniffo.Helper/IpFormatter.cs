using System.Net;

namespace Sniffo.Helper
{
    public static class IpFormatter
    {
        public static string Binary(string field, uint value, int binaryLength)
        {
            var padding = 50;
            var binary = Convert.ToString(value, 2).PadLeft(binaryLength, '0').PadLeft(32);
            return (field + ": ").PadLeft(padding) + binary + " = " + value;
        }

        public static string IPv4(string field, uint value)
        {
            var padding = 96;
            var ip = string.Join(".", new IPAddress(value)
                .ToString()
                .Split('.')
                .Select(block => block.PadLeft(3, ' ')).ToArray());
            return Binary(field, value, 32).PadRight(padding) + ip;
        }
    }
}