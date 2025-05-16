using System;
using System.Collections.Generic;
using System.IO;

namespace KM.Unpacker
{
    class ArchiveList
    {
        private static String m_Path = Utils.iGetApplicationPath() + @"\Projects\FileNames.list";

        private static Dictionary<String, String> m_HashList = new Dictionary<String, String>();

        public static void iLoadProject()
        {
            String m_Line = null;

            if (!File.Exists(m_Path))
            {
                Utils.iSetWarning("[WARNING]: Unable to load project file " + m_Path);
            }

            Int32 i = 0;
            m_HashList.Clear();

            StreamReader TProjectFile = new StreamReader(m_Path);
            while ((m_Line = TProjectFile.ReadLine()) != null)
            {
                UInt64[] dwHash = ArchiveHash.iGetStringHash(m_Line.ToLower());
                String m_Hash = dwHash[0].ToString("X16") + dwHash[1].ToString("X16");

                if (m_HashList.ContainsKey(m_Hash))
                {
                    String m_Collision = null;
                    m_HashList.TryGetValue(m_Hash, out m_Collision);

                    Utils.iSetError("[COLLISION]: " + m_Collision + " <-> " + m_Line);
                }

                m_HashList.Add(m_Hash, m_Line);
                i++;
            }

            TProjectFile.Close();
            Utils.iSetInfo("[INFO]: Project File Loaded: " + i.ToString());
            Console.WriteLine();
        }

        public static String iGetNameFromHashList(String m_Hash)
        {
            String m_FileName = null;

            if (m_HashList.ContainsKey(m_Hash))
            {
                m_HashList.TryGetValue(m_Hash, out m_FileName);
            }
            else
            {
                m_FileName = @"__Unknown/" + m_Hash;
            }

            return m_FileName;
        }
    }
}
