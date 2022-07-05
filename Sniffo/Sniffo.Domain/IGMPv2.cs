using Sniffo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sniffo.Domain
{
    public class IGMPv2 : Protocol
    {
        public IGMPType Type { get; private set; }
        public byte MaxRespTime { get; private set; }
        public ushort CheckSum { get; private set; }
        public UInt32 GroupAddress { get; private set; }

        public IGMPv2(byte[] data, int length) : base(length)
        {
            var stream = new BinaryReader(new MemoryStream(data));

            Type = (IGMPType)stream.ReadByte();

            MaxRespTime = stream.ReadByte();

            CheckSum = stream.ReadUInt16();

            GroupAddress = stream.ReadUInt32();
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("IGMPv2, " + Length + " bytes");
            result.AppendLine(IpFormatter.Binary("Type", (byte)Type, 8) + ", " + Type);
            result.AppendLine(IpFormatter.Binary("Unsed", MaxRespTime, 8));
            result.AppendLine(IpFormatter.Binary("CheckSum", CheckSum, 16));
            result.AppendLine(IpFormatter.IPv4("GroupAddress", GroupAddress));
            return result.ToString();
        }
    }
}
