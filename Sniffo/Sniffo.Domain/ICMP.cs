using Sniffo.Helper;
using System.Net;
using System.Text;

namespace Sniffo.Domain
{
    public class ICMP :Protocol
    {
        public ICMPType Type { get; private set; }
        public byte Code { get; private set; }
        public ushort Checksum { get; private set; }
        public uint RestOfHeader { get; private set; }
        public byte[] Data { get; private set; }

        public ICMP(byte[] data, int length) :base(length)
        {
            var stream = new BinaryReader(new MemoryStream(data));

            Type = (ICMPType)stream.ReadByte();
            Code = stream.ReadByte();
            Checksum = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());
            RestOfHeader = (uint)IPAddress.NetworkToHostOrder((int)stream.ReadUInt32());

            var dataOffsetInBytes = 64 / 8;
            Data = new byte[length - dataOffsetInBytes];
            Array.Copy(data, dataOffsetInBytes, Data, 0, Data.Length);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("ICMP, " + Length + " bytes");
            result.AppendLine(IpFormatter.Binary("Type", (byte)Type, 8) + ", " + Type);
            result.AppendLine(IpFormatter.Binary("Code", Code, 8));
            result.AppendLine(IpFormatter.Binary("Checksum", Checksum, 16));
            result.AppendLine(IpFormatter.Binary("RestOfHeader", RestOfHeader, 32));
            result.AppendLine(Encoding.Default.GetString(Data));
            return result.ToString();
        }
    }
}
