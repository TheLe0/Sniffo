namespace Sniffo.Domain
{
    public enum IGMPType : byte
    {
        MembershipQuery = 0x11,
        IGMPv1MembershipReport = 0x12,
        IGMPv2MembershipReport = 0x16,
        IGMPv3MembershipReport = 0x22,
        LeaveGroup = 0x17
    }
}
