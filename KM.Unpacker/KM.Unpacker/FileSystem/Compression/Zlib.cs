using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.AccessControl;
using System.Text;

namespace KM.Unpacker
{
    class Zlib
    {

        private static String iFromHexString(String m_HexString)
        {
            var lpTemp = new Byte[m_HexString.Length / 2];

            for (Int32 i = 0; i < lpTemp.Length; i++)
            {
                lpTemp[i] = Convert.ToByte(m_HexString.Substring(i * 2, 2), 16);
            }

            return Encoding.UTF8.GetString(lpTemp);
        }

        public static void iTryToDecompress(String m_FullPath, Byte[] lpBuffer, Int32 dwOffset = 2)
        {
            var TOutMemoryStream = new MemoryStream();
            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer))
            {
                Int32 dwAdditionalLength = 0;
                UInt32 dwMagic = TMemoryStream.ReadUInt32();

                if (dwMagic != 0xFACECAFE)
                {
                    if (m_FullPath.Contains("__Unknown"))
                    {
                        if (dwMagic == 0x90A0D7B || dwMagic == 0x200A0D7B)
                        {
                            m_FullPath += ".json";
                        }
                        else if (dwMagic == 0x4B504B41)
                        {
                            m_FullPath += ".cpk";
                        }
                    }

                    File.WriteAllBytes(m_FullPath, lpBuffer);

                    return;
                }

                Int32 dwOffset1 = TMemoryStream.ReadInt32();
                Int16 wUnknown1 = TMemoryStream.ReadInt16();
                Int16 wUnknown2 = TMemoryStream.ReadInt16();
                UInt32 dwResourceType = TMemoryStream.ReadUInt32(true);
                String m_ResourceType = iFromHexString(dwResourceType.ToString("X8"));

                if (m_ResourceType == "vdeo")
                {
                    dwAdditionalLength = 4;
                }

                if (m_FullPath.Contains("__Unknown"))
                {
                    m_FullPath += "." + m_ResourceType;
                }

                if (wUnknown2 != 1)
                {
                    Byte[] lpResult = new Byte[lpBuffer.Length - 42 - dwAdditionalLength];

                    Array.Copy(lpBuffer, 42 + dwAdditionalLength, lpResult, 0, lpResult.Length);

                    File.WriteAllBytes(m_FullPath, lpResult);

                    return;
                }

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

                lpBuffer = TOutMemoryStream.ToArray();

                if (m_ResourceType == "vdeo")
                {
                    Array.Copy(lpBuffer, dwAdditionalLength, lpBuffer, 0, lpBuffer.Length - dwAdditionalLength);
                }

                File.WriteAllBytes(m_FullPath, lpBuffer);
            }
        }
    }
}
