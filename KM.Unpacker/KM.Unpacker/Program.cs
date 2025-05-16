using System;
using System.IO;
using System.Text;

namespace KM.Unpacker
{
    class Program
    {
        private static String m_Title = "King of Meat (Technical Test) Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2025 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    KM.Unpacker <m_ArchiveFile> <m_Directory>\n");
                Console.WriteLine("    m_ArchiveFile - Source of Archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    KM.Unpacker E:\\Games\\KM\\archive\\data3.archive D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_ArchiveFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_ArchiveFile))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_ArchiveFile + " <- does not exist");
                return;
            }

            ArchiveUnpack.iDoIt(m_ArchiveFile, m_Output);
        }
    }
}
