using Sniffo.Helper;
using System.Net;
using System.Text;

namespace Sniffo.Domain
{
    public class TCP : Protocol
    {
        public ushort SourcePort { get; private set; }
        public ushort DestinationPort { get; private set; }
        public uint SequenceNumber { get; private set; }
        public uint AcknowledgmentNumber { get; private set; }
        public byte DataOffset { get; private set; }
        public byte Reserved { get; private set; }
        public ushort Flags { get; private set; }
        public ushort WindowSize { get; private set; }
        public ushort Checksum { get; private set; }
        public ushort UrgentPointer { get; private set; }
        public byte[] Data { get; private set; }

        public TCP(byte[] data, int length) :base(length)
        {
            var stream = new BinaryReader(new MemoryStream(data));

            int readed;

            SourcePort = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            DestinationPort = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            SequenceNumber = (uint)IPAddress.NetworkToHostOrder(stream.ReadInt32());

            AcknowledgmentNumber = (uint)IPAddress.NetworkToHostOrder((int)stream.ReadUInt32());

            readed = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());
            DataOffset = (byte)(readed >> 12);
            Reserved = (byte)((byte)(readed << 4) >> (4 + 9));
            Flags = (byte)((byte)(readed << 7) >> 7);

            WindowSize = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            Checksum = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            UrgentPointer = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            var dataOffsetInBytes = DataOffset * 32 / 8;
            Data = new byte[length - dataOffsetInBytes];
            Array.Copy(data, dataOffsetInBytes, Data, 0, Data.Length);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("TCP, " + Length + " bytes");
            result.AppendLine(IpFormatter.Binary("SourcePort", SourcePort, 16));
            result.AppendLine(IpFormatter.Binary("DestinationPort", DestinationPort, 16));
            result.AppendLine(IpFormatter.Binary("SequenceNumber", SequenceNumber, 32));
            result.AppendLine(IpFormatter.Binary("AcknowledgmentNumber", AcknowledgmentNumber, 32));
            result.AppendLine(IpFormatter.Binary("DataOffset", DataOffset, 4));
            result.AppendLine(IpFormatter.Binary("Reserved", Reserved, 3));
            result.AppendLine(IpFormatter.Binary("Flags", Flags, 9));
            result.AppendLine(IpFormatter.Binary("WindowSize", WindowSize, 16));
            result.AppendLine(IpFormatter.Binary("Checksum", Checksum, 16));
            result.AppendLine(IpFormatter.Binary("UrgentPointer", UrgentPointer, 16));
            result.AppendLine(Encoding.Default.GetString(Data));
            return result.ToString();
        }
    }
}
