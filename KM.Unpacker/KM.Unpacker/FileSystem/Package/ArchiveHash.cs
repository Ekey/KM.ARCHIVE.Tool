using System;
using System.IO;
using System.Text;

namespace KM.Unpacker
{
    class ArchiveHash
    {
        //data5.archive
        //assets/ui/icons/navigation_arrow_condensed.png
        //D8C83FBF91C295A33BFCFF1D89EB8EC7

        //data3.archive
        //assets/ui/kom_ui_workspace.louie.atlas_1.png
        //3C481E613B64298556278B9F1416CD4

        public static UInt64[] iGetStringHash(String m_String, UInt32 dwSeed = 0xA9401E9F)
        {
            UInt64[] dwHash;
            Byte[] lpBuffer = Encoding.UTF8.GetBytes(m_String);

            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer))
            {
                dwHash = Murmur3.HashCore64(lpBuffer, dwSeed);
            }

            return dwHash;
        }
    }
}
