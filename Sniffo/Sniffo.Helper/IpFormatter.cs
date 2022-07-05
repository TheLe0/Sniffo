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

        public static string IPv6(string field, uint _addressA, uint _addressB, uint _addressC, uint _addressD)
        {
            String binary;
            var padding = 50;
            var b1 = BitConverter.GetBytes(_addressA);
            var b2 = BitConverter.GetBytes(_addressB);
            var b3 = BitConverter.GetBytes(_addressC);
            var b4 = BitConverter.GetBytes(_addressD);
            /*var bytes = new byte[] {
                        b1[0], b1[1], b1[2], b1[3],
                        b2[0], b2[1], b2[2], b2[3],
                        b3[0], b3[1], b3[2], b3[3],
                        b4[0], b4[1], b4[2], b4[3]
                    };*/
            var bytes = new byte[] {
                        b1[3], b1[2], b1[1], b1[0],
                        b2[3], b2[2], b2[1], b2[0],
                        b3[3], b3[2], b3[1], b3[0],
                        b4[3], b4[2], b4[1], b4[0]
                    };
            var ip = new IPAddress(bytes);

            binary = Convert.ToString(_addressA, 2).PadLeft(32, '0').PadLeft(32) + "\n" +
                     Convert.ToString(_addressB, 2).PadLeft(32, '0').PadLeft(82) + "\n" +
                     Convert.ToString(_addressC, 2).PadLeft(32, '0').PadLeft(82) + "\n" +
                     Convert.ToString(_addressD, 2).PadLeft(32, '0').PadLeft(82);

            return (field + ": ").PadLeft(padding) + binary + " = " + ip;
        }

        public static string IPv6(string field, IPAddress _IPAddress)
        {
            
            return (field + ": ").PadLeft(0) + " = " + _IPAddress;
        }


        /// Referencia: https://www.iana.org/assignments/protocol-numbers/protocol-numbers.xhtml#protocol-numbers-1
        public static string NextHeader(byte _nextHeader)
        {
            var padding = 50;
            string description;

            description = _nextHeader switch
            {
                0 => "HOPOPT -> IPv6 Hop-by-Hop Option",
                1 => "ICMP -> Internet Control Message",
                2 => "IGMP -> Internet Group Management",
                3 => "GGP -> Gateway-to-Gateway",
                4 => "IPv4 -> IPv4 encapsulation",
                5 => "ST -> Stream",
                6 => "TCP -> Transmission Control",
                7 => "CBT -> CBT",
                8 => "EGP -> Exterior Gateway Protocol",
                9 => "IGP -> any private interior gateway(used by Cisco for their IGRP)",
                10 => "BBN-RCC-MON -> BBN RCC Monitoring",
                11 => "NVP-II -> Network Voice Protocol",
                12 => "PUP -> PUP",
                13 => "ARGUS(deprecated) -> ARGUS",
                14 => "EMCON -> EMCON",
                15 => "XNET -> Cross Net Debugger",
                16 => "CHAOS -> Chaos",
                17 => "UDP -> User Datagram",
                18 => "MUX -> Multiplexing",
                19 => "DCN-MEAS -> DCN Measurement Subsystems",
                20 => "HMP -> Host Monitoring",
                21 => "PRM -> Packet Radio Measurement",
                22 => "XNS-IDP -> XEROX NS IDP",
                23 => "TRUNK-1	-> Trunk-1",
                24 => "TRUNK-2	-> Trunk-2",
                25 => "LEAF-1	-> Leaf-1",
                26 => "LEAF-2	-> Leaf-2",
                27 => "RDP -> Reliable Data Protocol",
                28 => "IRTP -> Internet Reliable Transaction",
                29 => "ISO-TP4 -> ISO Transport Protocol Class 4",
                30 => "NETBLT -> Bulk Data Transfer Protocol",
                31 => "MFE-NSP -> MFE Network Services Protocol",
                32 => "MERIT-INP -> MERIT Internodal Protocol",
                33 => "DCCP -> Datagram Congestion Control Protocol",
                34 => "3PC -> Third Party Connect Protocol",
                35 => "IDPR -> Inter-Domain Policy Routing Protocol",
                36 => "XTP -> XTP",
                37 => "DDP -> Datagram Delivery Protocol",
                38 => "IDPR-CMTP -> IDPR Control Message Transport Proto",
                39 => "TP++	-> TP++ Transport Protocol",
                40 => "IL -> IL Transport Protocol",
                41 => "IPv6 -> IPv6 encapsulation",
                42 => "SDRP -> Source Demand Routing Protocol",
                43 => "IPv6-Route -> Routing Header for IPv6 Y[Steve_Deering]",
                44 => "IPv6-Frag -> Fragment Header for IPv6 Y[Steve_Deering]",
                45 => "IDRP -> Inter-Domain Routing Protocol",
                46 => "RSVP -> Reservation Protocol",
                47 => "GRE -> Generic Routing Encapsulation",
                48 => "DSR -> Dynamic Source Routing Protocol",
                49 => "BNA -> BNA",
                50 => "ESP -> Encap Security Payload  Y[RFC4303]",
                51 => "AH -> Authentication Header Y[RFC4302]",
                52 => "I-NLSP -> Integrated Net Layer Security TUBA",
                53 => "SWIPE(deprecated) -> IP with Encryption",
                54 => "NARP -> NBMA Address Resolution Protocol",
                55 => "MOBILE -> IP Mobility",
                56 => "TLSP -> Transport Layer Security Protocol using Kryptonet key management",
                57 => "SKIP -> SKIP",
                58 => "IPv6-ICMP -> ICMP for IPv6",
                59 => "IPv6-NoNxt -> No Next Header for IPv6",
                60 => "IPv6-Opts -> Destination Options for IPv6 Y[RFC8200]",
                62 => "CFTP -> CFTP",
                64 => "SAT-EXPAK -> SATNET and Backroom EXPAK",
                65 => "KRYPTOLAN -> Kryptolan",
                66 => "RVD -> MIT Remote Virtual Disk Protocol",
                67 => "IPPC -> Internet Pluribus Packet Core",
                69 => "SAT-MON -> SATNET Monitoring",
                70 => "VISA -> VISA Protocol",
                71 => "IPCV -> Internet Packet Core Utility",
                72 => "CPNX -> Computer Protocol Network Executive",
                73 => "CPHB -> Computer Protocol Heart Beat",
                74 => "WSN -> Wang Span Network",
                75 => "PVP -> Packet Video Protocol",
                76 => "BR-SAT-MON -> Backroom SATNET Monitoring",
                77 => "SUN-ND -> SUN ND PROTOCOL-Temporary",
                78 => "WB-MON -> WIDEBAND Monitoring",
                79 => "WB-EXPAK -> WIDEBAND EXPAK",
                80 => "ISO-IP -> ISO Internet Protocol",
                81 => "VMTP -> VMTP",
                82 => "SECURE-VMTP -> SECURE-VMTP",
                83 => "VINES -> VINES",
                84 => "TTP -> Transaction Transport Protocol | IPTM -> Internet Protocol Traffic Manager",
                85 => "NSFNET-IGP -> NSFNET-IGP",
                86 => "DGP -> Dissimilar Gateway Protocol",
                87 => "TCF -> TCF",
                88 => "EIGRP -> EIGRP",
                89 => "OSPFIGP -> OSPFIGP",
                90 => "Sprite-RPC -> Sprite RPC Protocol",
                91 => "LARP -> Locus Address Resolution Protocol",
                92 => "MTP -> Multicast Transport Protocol",
                93 => "AX.25 -> AX.25 Frames",
                94 => "IPIP -> IP-within-IP Encapsulation Protocol",
                95 => "MICP(deprecated)  -> Mobile Internetworking Control Pro.",
                96 => "SCC-SP -> Semaphore Communications Sec. Pro.",
                97 => "ETHERIP -> Ethernet-within-IP Encapsulation",
                98 => "ENCAP -> Encapsulation Header",
                100 => "GMTP -> GMTP",
                101 => "IFMP -> Ipsilon Flow Management Protocol",
                102 => "PNNI -> PNNI over IP",
                103 => "PIM -> Protocol Independent Multicast",
                104 => "ARIS -> ARIS",
                105 => "SCPS -> SCPS",
                106 => "QNX -> QNX",
                107 => "A/N -> Active Networks",
                108 => "IPComp -> IP Payload Compression Protocol",
                109 => "SNP -> Sitara Networks Protocol",
                110 => "Compaq-Peer -> Compaq Peer Protocol",
                111 => "IPX-in-IP -> IPX in IP",
                112 => "VRRP -> Virtual Router Redundancy Protocol",
                113 => "PGM -> PGM Reliable Transport Protocol",
                115 => "L2TP -> Layer Two Tunneling Protocol",
                116 => "DDX -> D-II Data Exchange (DDX)",
                117 => "IATP -> Interactive Agent Transfer Protocol",
                118 => "STP -> Schedule Transfer Protocol",
                119 => "SRP -> SpectraLink Radio Protocol",
                120 => "UTI -> UTI",
                121 => "SMP -> Simple Message Protocol",
                122 => "SM (deprecated) -> Simple Multicast Protocol",
                123 => "PTP -> Performance Transparency Protocol",
                124 => "ISIS -> over IPv4",
                125 => "FIRE",
                126 => "CRTP -> Combat Radio Transport Protocol",
                127 => "CRUDP -> Combat Radio User Datagram",
                128 => "SSCOPMCE",
                129 => "IPLT",
                130 => "SPS -> Secure Packet Shield",
                131 => "PIPE -> Private IP Encapsulation within IP",
                132 => "SCTP -> Stream Control Transmission Protocol",
                133 => "FC -> Fibre Channel",
                134 => "RSVP-E2E-IGNORE",
                135 => "Mobility Header",
                136 => "UDPLite",
                137 => "MPLS-in-IP",
                138 => "manet -> MANET Protocols",
                139 => "HIP -> Host Identity Protocol  Y[RFC7401]",
                140 => "Shim6 -> Shim6 Protocol Y   [RFC5533]",
                141 => "WESP -> Wrapped Encapsulating Security Payload",
                142 => "ROHC -> Robust Header Compression",
                143 => "Ethernet -> Ethernet",
                _ => throw new NotImplementedException()
            };

            return ("Next Header Type: ").PadLeft(padding) + description.PadLeft(32);
        }
    }
}