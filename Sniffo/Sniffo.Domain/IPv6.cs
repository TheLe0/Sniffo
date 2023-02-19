using Sniffo.Helper;
using System.Net;
using System.Text;

namespace Sniffo.Domain
{
    public class IPv6 : Protocol
    {
        public byte Version { get; private set; }
        public byte TrafficClass { get; private set; }
        public uint FlowLabel { get; private set; }
        public ushort PayloadLength { get; private set; }
        public IPProtocolType NextHeader { get; private set; }
        public byte HopLimit { get; private set; }
        public uint SourceAddressA { get; private set; }
        public uint SourceAddressB { get; private set; }
        public uint SourceAddressC { get; private set; }
        public uint SourceAddressD { get; private set; }
        public uint DestinationAddressA { get; private set; }
        public uint DestinationAddressB { get; private set; }
        public uint DestinationAddressC { get; private set; }
        public uint DestinationAddressD { get; private set; }

        public byte[] Data { get; private set; }
        public object? DataAsProtocol { get; private set; }

        
        public IPv6(byte[] data, int length) : base(length)
        {
            var stream = new BinaryReader(new MemoryStream(data));

            uint readed;

            readed = (uint) stream.ReadInt32();

            Version = (byte)(readed >> 4);

            TrafficClass = (byte)((byte)(readed << 4) >> 8);

            FlowLabel = (byte)((byte)(readed << 12) >> 20);

            PayloadLength = stream.ReadUInt16();

            NextHeader = (IPProtocolType) stream.ReadByte();

            HopLimit = stream.ReadByte();

            SourceAddressA = stream.ReadUInt32();
            SourceAddressB = stream.ReadUInt32();
            SourceAddressC = stream.ReadUInt32();
            SourceAddressD = stream.ReadUInt32();

            DestinationAddressA = stream.ReadUInt32();
            DestinationAddressB = stream.ReadUInt32();
            DestinationAddressC = stream.ReadUInt32();
            DestinationAddressD = stream.ReadUInt32();

            var internetHeaderLengthInBytes = 320 / 8;
            Data = new byte[PayloadLength / 8];
            Array.Copy(data, internetHeaderLengthInBytes, Data, 0, Data.Length);

            DataAsProtocol = NextHeader switch
            {
                IPProtocolType.TCP => new TCP(Data, Data.Length),
                IPProtocolType.ICMP => new ICMP(Data, Data.Length),
                IPProtocolType.UDP => new UDP(Data, Data.Length),
                IPProtocolType.IGMP => new IGMPv2(Data, Data.Length),
                _ => null,
            };
        }
        
        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("IPv6, " + Length + " bytes");
            result.AppendLine(IpFormatter.Binary("Version", Version, 8));
            result.AppendLine(IpFormatter.Binary("TrafficClass", TrafficClass, 8));
            result.AppendLine(IpFormatter.Binary("FlowLabel", FlowLabel, 16));
            result.AppendLine(IpFormatter.Binary("PayloadLength", PayloadLength, 16));
            result.AppendLine(IpFormatter.Binary("NextHeader", (uint)NextHeader, 8));
            result.AppendLine(IpFormatter.Binary("HopLimit", HopLimit, 8));
            result.AppendLine(IpFormatter.IPv6("SourceAddress", SourceAddressA, SourceAddressB, SourceAddressC, SourceAddressD));
            result.AppendLine(IpFormatter.IPv6("DestinationAddress", DestinationAddressA, DestinationAddressB, DestinationAddressC, DestinationAddressD));
            result.AppendLine(IpFormatter.NextHeader((byte)NextHeader));

            return result.ToString();
        }
    }
}