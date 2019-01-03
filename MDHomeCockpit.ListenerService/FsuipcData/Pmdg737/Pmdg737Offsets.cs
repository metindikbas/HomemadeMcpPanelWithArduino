using System;
using FSUIPC;

namespace MDHomeCockpit.ListenerService.FsuipcData.Pmdg737
{
    internal class Pmdg737Offsets
    {
        public static readonly Offset<Int16> MCP_Course = new Offset<Int16>("MCP", 0x6520);
        public static readonly Offset<Int16> MCP_Course2 = new Offset<Int16>("MCP", 0x6522);
        public static readonly Offset<float> MCP_IASMach = new Offset<float>("MCP", 0x6524);
        public static readonly Offset<byte> MCP_IASBlank = new Offset<byte>("MCP", 0x6528);
        public static readonly Offset<Int16> MCP_Heading = new Offset<Int16>("MCP", 0x652C);
        public static readonly Offset<UInt16> MCP_Altitude = new Offset<UInt16>("MCP", 0x652E);
        public static readonly Offset<short> MCP_VertSpeed = new Offset<short>("MCP", 0x6530);
        public static readonly Offset<byte> MCP_VertSpeedBlank = new Offset<byte>("MCP", 0x6532);
        public static readonly Offset<byte> MCP_FDSw = new Offset<byte>("MCP", 0x6533);
        public static readonly Offset<byte> MCP_FDSw2 = new Offset<byte>("MCP", 0x6534);
        public static readonly Offset<byte> MCP_ATArmSw = new Offset<byte>("MCP", 0x6535);
        public static readonly Offset<byte> MCP_DisengageBar = new Offset<byte>("MCP", 0x6537);
        public static readonly Offset<byte> MCP_annunN1 = new Offset<byte>("MCP", 0x653B);
        public static readonly Offset<byte> MCP_annunSPEED = new Offset<byte>("MCP", 0x653C);
        public static readonly Offset<byte> MCP_annunVNAV = new Offset<byte>("MCP", 0x653D);
        public static readonly Offset<byte> MCP_annunLVL_CHG = new Offset<byte>("MCP", 0x653E);
        public static readonly Offset<byte> MCP_annunHDG_SEL = new Offset<byte>("MCP", 0x653F);
        public static readonly Offset<byte> MCP_annunLNAV = new Offset<byte>("MCP", 0x6540);
        public static readonly Offset<byte> MCP_annunVOR_LOC = new Offset<byte>("MCP", 0x6541);
        public static readonly Offset<byte> MCP_annunAPP = new Offset<byte>("MCP", 0x6542);
        public static readonly Offset<byte> MCP_annunALT_HOLD = new Offset<byte>("MCP", 0x6543);
        public static readonly Offset<byte> MCP_annunVS = new Offset<byte>("MCP", 0x6544);
        public static readonly Offset<byte> MCP_annunCMD_A = new Offset<byte>("MCP", 0x6545);
        public static readonly Offset<byte> MCP_annunCWS_A = new Offset<byte>("MCP", 0x6546);
        public static readonly Offset<byte> MCP_annunCMD_B = new Offset<byte>("MCP", 0x6547);
        public static readonly Offset<byte> MCP_annunCWS_B = new Offset<byte>("MCP", 0x6548);
    }
}
