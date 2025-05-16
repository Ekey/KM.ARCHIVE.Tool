using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Remoting.Messaging;

namespace KM.Unpacker
{
    class Zlib
    {
        public static Byte[] iTryDecompress(Byte[] lpBuffer, Int32 dwOffset = 2)
        {
            var TOutMemoryStream = new MemoryStream();
            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer))
            {
                UInt32 dwMagic = TMemoryStream.ReadUInt32();

                if (dwMagic != 0xFACECAFE)
                {
                    return lpBuffer;
                }

                Int32 dwOffset1 = TMemoryStream.ReadInt32();
                Int16 wUnknown1 = TMemoryStream.ReadInt16();
                Int16 wUnknown2 = TMemoryStream.ReadInt16();

                if (wUnknown2 != 1)
                {
                    Byte[] lpResult = new Byte[lpBuffer.Length - 42];

                    Array.Copy(lpBuffer, 42, lpResult, 0, lpResult.Length);

                    return lpResult;
                }

                UInt32 dwResourceType = TMemoryStream.ReadUInt32();

                Int32 dwDataSize = TMemoryStream.ReadInt32();
                Int32 dwUnknown1 = TMemoryStream.ReadInt32();
                Int32 dwOffset2 = TMemoryStream.ReadInt32();
                Int32 dwCompressedSize = TMemoryStream.ReadInt32();

                TMemoryStream.Seek(dwOffset2 + 2, SeekOrigin.Begin);

                using (DeflateStream TDeflateStream = new DeflateStream(TMemoryStream, CompressionMode.Decompress, false))
                {
                    TDeflateStream.CopyTo(TOutMemoryStream);
                    TDeflateStream.Dispose();
                }
                TMemoryStream.Dispose();
            }

            return TOutMemoryStream.ToArray();
        }
    }
}
