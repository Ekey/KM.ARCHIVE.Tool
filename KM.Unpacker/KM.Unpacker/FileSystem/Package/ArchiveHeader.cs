using System;

namespace KM.Unpacker
{
    class ArchiveHeader
    {
        public UInt32 dwMagic { get; set; } // gpak (0x6B617067)
        public UInt16 wMinorVersion { get; set; } // 2
        public UInt16 wMajorVersion { get; set; } // 32
        public Int32 dwSerialiseRevision { get; set; } // 47
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwArchiveParts { get; set; } // 6
        public Int32 dwBaseOffset { get; set; }
        public Int32 dwFilesDataSize { get; set; }
        public Int32 dwReserved { get; set; } // 0
    }
}
