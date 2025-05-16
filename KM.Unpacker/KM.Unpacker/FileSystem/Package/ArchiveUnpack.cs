using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;

namespace KM.Unpacker
{
    class ArchiveUnpack
    {
        private static List<ArchiveEntry> m_EntryTable = new List<ArchiveEntry>();

        public static void iDoIt(String m_ArchiveFile, String m_DstFolder)
        {
            ArchiveList.iLoadProject();

            using (FileStream TArchiveStream = File.OpenRead(m_ArchiveFile))
            {
                var m_Header = new ArchiveHeader();

                m_Header.dwMagic = TArchiveStream.ReadUInt32();

                if (m_Header.dwMagic != 0x6B617067)
                {
                    throw new Exception("[ERROR]: Invalid magic of Archive file!");
                }

                m_Header.wMajorVersion = TArchiveStream.ReadUInt16();
                m_Header.wMinorVersion = TArchiveStream.ReadUInt16();
                m_Header.dwSerialiseRevision = TArchiveStream.ReadInt32();

                if (m_Header.wMajorVersion != 2 && m_Header.wMinorVersion != 32 && m_Header.dwSerialiseRevision != 47)
                {
                    throw new Exception("[ERROR]: Invalid Archive file!");
                }

                m_Header.dwTotalFiles = TArchiveStream.ReadInt32();
                m_Header.dwArchiveParts = TArchiveStream.ReadInt32();
                m_Header.dwBaseOffset = TArchiveStream.ReadInt32();
                m_Header.dwFilesDataSize = TArchiveStream.ReadInt32();
                m_Header.dwReserved = TArchiveStream.ReadInt32();

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new ArchiveEntry();

                    m_Entry.dwHash1 = TArchiveStream.ReadUInt64();
                    m_Entry.dwHash2 = TArchiveStream.ReadUInt64();

                    m_EntryTable.Add(m_Entry);
                }

                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    m_EntryTable[i].dwOffset = TArchiveStream.ReadInt64();
                    m_EntryTable[i].dwSize = TArchiveStream.ReadInt64();
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = ArchiveList.iGetNameFromHashList(m_Entry.dwHash1.ToString("X16") + m_Entry.dwHash2.ToString("X16")).Replace("/", @"\");
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TArchiveStream.Seek(m_Entry.dwOffset + m_Header.dwBaseOffset, SeekOrigin.Begin);

                    var lpBuffer = TArchiveStream.ReadBytes((Int32)m_Entry.dwSize);

                    lpBuffer = Zlib.iTryDecompress(lpBuffer);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TArchiveStream.Dispose();
            }
        }
    }
}
