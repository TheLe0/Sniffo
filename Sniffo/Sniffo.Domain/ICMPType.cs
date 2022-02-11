using System;
namespace Sniffo.Domain
{
    public enum ICMPType : byte
    {
        EchoReply = 0,
        DestinationUnreachable = 3,
        SourceQuench = 4,
        RedirectMessage = 5,
        EchoRequest = 8,
        RouterAdvertisement = 9,
        RouterSolicitation = 10,
        TimeExceeded = 11,
        ParameterProblem = 12,
        Timestamp = 13,
        TimestampReply = 14,
        InformationRequest = 15,
        InformationReply = 16,
        AddressMaskRequest = 17,
        AddressMaskReply = 18,
        ExtendedEchoRequest = 42,
        ExtendedEchoReply = 43
    }
}
