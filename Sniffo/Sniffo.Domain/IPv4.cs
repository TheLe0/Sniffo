using Sniffo.Helper;
using System.Net;
using System.Text;

namespace Sniffo.Domain
{
    public class IPv4 :Protocol
    {
        public byte Version { get; private set; }
        public byte InternetHeaderLength { get; private set; }
        public byte DifferentiatedServicesCodePoint { get; private set; }
        public byte ExplicitCongestionNotification { get; private set; }
        public ushort TotalLength { get; private set; }
        public ushort Identification { get; private set; }
        public byte Flags { get; private set; }
        public ushort FragmentOffset { get; private set; }
        public byte TimeToLive { get; private set; }
        public IPProtocolType Protocol { get; private set; }
        public ushort HeaderChecksum { get; private set; }
        public uint SourceIPAddress { get; private set; }
        public uint DestinationIPAddress { get; private set; }
        public byte[] Data { get; private set; }
        public object? DataAsProtocol { get; private set; }

        public IPv4(byte[] data, int length) :base(length)
        {
            var stream = new BinaryReader(new MemoryStream(data));

            int readed;

            readed = stream.ReadByte();
            Version = (byte)(readed >> 4);
            InternetHeaderLength = (byte)((byte)(readed << 4) >> 4);

            readed = stream.ReadByte();
            DifferentiatedServicesCodePoint = (byte)(readed >> 2);
            ExplicitCongestionNotification = (byte)((byte)(readed << 6) >> 6);

            TotalLength = stream.ReadUInt16();

            if (BitConverter.IsLittleEndian)
            {
                TotalLength = (ushort)((TotalLength >> 8) + (TotalLength << 8));
            }

            Identification = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            readed = IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());
            Flags = (byte)(readed >> 13);
            FragmentOffset = (ushort)((ushort)(readed << 3) >> 3);

            TimeToLive = stream.ReadByte();

            Protocol = (IPProtocolType)stream.ReadByte();

            HeaderChecksum = (ushort)IPAddress.NetworkToHostOrder((short)stream.ReadUInt16());

            SourceIPAddress = (uint)stream.ReadInt32();

            DestinationIPAddress = (uint)stream.ReadInt32();

            var internetHeaderLengthInBytes = InternetHeaderLength * 32 / 8;
            Data = new byte[TotalLength - internetHeaderLengthInBytes];
            Array.Copy(data, internetHeaderLengthInBytes, Data, 0, Data.Length);

            DataAsProtocol = Protocol switch
            {
                IPProtocolType.TCP => new TCP(Data, Data.Length),
                IPProtocolType.ICMP => new ICMP(Data, Data.Length),
                _ => null,
            };
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine("IPv4, " + Length + " bytes");
            result.AppendLine(IpFormatter.Binary("Version", Version, 4));
            result.AppendLine(IpFormatter.Binary("InternetHeaderLength", InternetHeaderLength, 4));
            result.AppendLine(IpFormatter.Binary("DifferentiatedServicesCodePoint", DifferentiatedServicesCodePoint, 6));
            result.AppendLine(IpFormatter.Binary("ExplicitCongestionNotification", ExplicitCongestionNotification, 2));
            result.AppendLine(IpFormatter.Binary("TotalLength", TotalLength, 16));
            result.AppendLine(IpFormatter.Binary("Identification", Identification, 16));
            result.AppendLine(IpFormatter.Binary("Flags", Flags, 3));
            result.AppendLine(IpFormatter.Binary("FragmentOffset", FragmentOffset, 13));
            result.AppendLine(IpFormatter.Binary("TimeToLive", TimeToLive, 8));
            result.AppendLine(IpFormatter.Binary("Protocol", (byte)Protocol, 8) + ", " + Protocol);
            result.AppendLine(IpFormatter.Binary("HeaderChecksum", HeaderChecksum, 16));
            result.AppendLine(IpFormatter.IPv4("SourceIPAddress", SourceIPAddress));
            result.AppendLine(IpFormatter.IPv4("DestinationIPAddress", DestinationIPAddress));

            result.AppendLine(
                DataAsProtocol != null
                    ? DataAsProtocol.ToString()
                    : Encoding.Default.GetString(Data));

            return result.ToString();
        }
    }
}
