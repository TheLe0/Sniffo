using Sniffo.Helper;
using System.Net;
using System.Text;

namespace Sniffo.Domain
{
    public class UDP : Protocol
    {
        public ushort SourcePort { get; private set; }
        public ushort DestinationPort { get; private set; }
        public ushort Lenght { get; private set; }
        public ushort CheckSum { get; private set; }
        public byte[] Data { get; private set; }

        public UDP(byte[] data, int length) : base(length)
        {
            var stream = new BinaryReader(new MemoryStream(data));

            SourcePort = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            DestinationPort = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            Lenght = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            CheckSum = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            Data = new byte[Lenght - 64 / 8];
            Array.Copy(data, 8, Data, 0, Data.Length);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("UDP, " + Length + " bytes");
            result.AppendLine(IpFormatter.Binary("SourcePort", SourcePort, 16));
            result.AppendLine(IpFormatter.Binary("DestinationPort", DestinationPort, 16));
            result.AppendLine(IpFormatter.Binary("Lenght", Lenght, 16));
            result.AppendLine(IpFormatter.Binary("CheckSum", CheckSum, 16));
            result.AppendLine(Encoding.Default.GetString(Data));
            return result.ToString();
        }
    }
}
