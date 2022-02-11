using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sniffo.Domain;
using Sniffo.Helper;

namespace Sniffo.Test
{
    [TestClass]
    public class IpFormatterTest
    {
        [TestMethod]
        public void Ipv4ParsingTest()
        {
            byte _version = 4;
            byte _internetHeaderLength = 5;
            byte _differentiatedServicesCodePoint = 0;
            byte _explicitCongestionNotification = 0;
            ushort _totalLength = 69;
            ushort _identification = 30632;
            byte _flags = 2;
            ushort _fragmentOffset = 0;
            byte _timeToLive = 128;
            IPProtocolType _protocol = IPProtocolType.TCP;
            ushort _headerChecksum = 31798;
            uint _sourceIPAddress = 1761607690;
            uint _destinationIPAddress = 426791564;

            Assert.AreNotEqual(IpFormatter.Binary("Version", _version, 4), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("InternetHeaderLength", _internetHeaderLength, 4), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("DifferentiatedServicesCodePoint", _differentiatedServicesCodePoint, 6), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("ExplicitCongestionNotification", _explicitCongestionNotification, 2), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("TotalLength", _totalLength, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Identification", _identification, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Flags", _flags, 3), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("FragmentOffset", _fragmentOffset, 13), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("TimeToLive", _timeToLive, 8), string.Empty);
            Assert.AreNotEqual((IpFormatter.Binary("Protocol", (byte)_protocol, 8) + ", " + _protocol), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("HeaderChecksum", _headerChecksum, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.IPv4("SourceIPAddress", _sourceIPAddress), string.Empty);
            Assert.AreNotEqual(IpFormatter.IPv4("DestinationIPAddress", _destinationIPAddress), string.Empty);
        }

        [TestMethod]
        public void TcpParsingTest()
        {
            ushort _sourcePort = 61256;
            ushort _destinationPort = 443;
            uint _sequenceNumber = 612684663;
            uint _acknowledgmentNumber = 0;
            byte _dataOffset = 8;
            byte _reserved = 0;
            ushort _flags = 0;
            ushort _windowSize = 64240;
            ushort _checksum = 17959;
            ushort _urgentPointer = 0;

            Assert.AreNotEqual(IpFormatter.Binary("SourcePort", _sourcePort, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("DestinationPort", _destinationPort, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("SequenceNumber", _sequenceNumber, 32), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("AcknowledgmentNumber", _acknowledgmentNumber, 32), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("DataOffset", _dataOffset, 4), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Reserved", _reserved, 3), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Flags", _flags, 9), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("WindowSize", _windowSize, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Checksum", _checksum, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("UrgentPointer", _urgentPointer, 16), string.Empty);
        }

        [TestMethod]
        public void IcmpParsingTest()
        {
            ICMPType _type = ICMPType.EchoRequest;
            byte _code = 0;
            ushort _checksum = 17959;
            uint _restOfHeader = 0;

            Assert.AreNotEqual(IpFormatter.Binary("Type", (byte)_type, 8) + ", " + _type, string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Code", _code, 8), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("Checksum", _checksum, 16), string.Empty);
            Assert.AreNotEqual(IpFormatter.Binary("RestOfHeader", _restOfHeader, 32), string.Empty);
        }
    }
}