using System;

namespace KM.Unpacker
{
    class ArchiveEntry
    {
        public UInt64 dwHash1 { get; set; } // MurMur3 hash p1 (seed 0xA9401E9F)
        public UInt64 dwHash2 { get; set; } // MurMur3 hash p2 (seed 0xA9401E9F)
        public Int64 dwOffset { get; set; }
        public Int64 dwSize { get; set; }
    }
}
