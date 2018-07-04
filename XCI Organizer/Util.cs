using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCI_Organizer {
    internal static class Util {

        public static string GetMkey(byte id)
        {
            switch (id)
            {
                case 0:
                case 1:
                    return "MasterKey0 (1.0.0-2.3.0)";
                case 2:
                    return "MasterKey1 (3.0.0)";
                case 3:
                    return "MasterKey2 (3.0.1-3.0.2)";
                case 4:
                    return "MasterKey3 (4.0.0-4.1.0)";
                case 5:
                    return "MasterKey4 (5.0.0+)";
                case 6:
                    return "MasterKey5 (?)";
                case 7:
                    return "MasterKey6 (?)";
                case 8:
                    return "MasterKey7 (?)";
                case 9:
                    return "MasterKey8 (?)";
                case 10:
                    return "MasterKey9 (?)";
                case 11:
                    return "MasterKey10 (?)";
                case 12:
                    return "MasterKey11 (?)";
                case 13:
                    return "MasterKey12 (?)";
                case 14:
                    return "MasterKey13 (?)";
                case 15:
                    return "MasterKey14 (?)";
                case 16:
                    return "MasterKey15 (?)";
                case 17:
                    return "MasterKey16 (?)";
                case 18:
                    return "MasterKey17 (?)";
                case 19:
                    return "MasterKey18 (?)";
                case 20:
                    return "MasterKey19 (?)";
                case 21:
                    return "MasterKey20 (?)";
                case 22:
                    return "MasterKey21 (?)";
                case 23:
                    return "MasterKey22 (?)";
                case 24:
                    return "MasterKey23 (?)";
                case 25:
                    return "MasterKey24 (?)";
                case 26:
                    return "MasterKey25 (?)";
                case 27:
                    return "MasterKey26 (?)";
                case 28:
                    return "MasterKey27 (?)";
                case 29:
                    return "MasterKey28 (?)";
                case 30:
                    return "MasterKey29 (?)";
                case 31:
                    return "MasterKey30 (?)";
                case 32:
                    return "MasterKey31 (?)";
                case 33:
                    return "MasterKey32 (?)";
                default:
                    return "?";
            }
        }

        public static List<string> GetXCIsInFolder(string folder) {
            List<string> list = new List<string>();

            try {
                foreach (string f in Directory.GetFiles(folder, "*.xci", SearchOption.AllDirectories)) {
                    list.Add(f);
                }
            } catch (System.Exception execpt) {
                Console.WriteLine(execpt.Message);
            }

            return list;
        }

        public static byte[] StringToByteArray(string hex) {
            return (from x in Enumerable.Range(0, hex.Length)
                    where x % 2 == 0
                    select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static bool CheckXCI(string file) {
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[61440];
            byte[] array2 = new byte[16];
            fileStream.Read(array, 0, 61440);
            XCI.XCI_Headers[0] = new XCI.XCI_Header(array);
            if (!XCI.XCI_Headers[0].Magic.Contains("HEAD")) {
                return false;
            }
            fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition;
            fileStream.Read(array2, 0, 16);
            HFS0.HFS0_Headers[0] = new HFS0.HFS0_Header(array2);
            fileStream.Close();
            return true;
        }

        public static string GetCapacity(int id) {
            switch (id)
            {
                case 248:
                    return "2GB";
                case 240:
                    return "4GB";
                case 224:
                    return "8GB";
                case 225:
                    return "16GB";
                case 226:
                    return "32GB";
                default:
                    return "?";
            }
        }

        public static bool checkFile(string filepath) {
            if (File.Exists(filepath)) {
                return true;
            }
            return false;
        }

        public static bool RenameFile(string filepath, string newName)
        {
            if (checkFile(filepath))
            {
                string uncheckedName = newName;
                List<char> invalidChars = new List<char>();
                string _newName;
                string newPath;

                // Add characters to remove from filename here
                invalidChars.AddRange(Path.GetInvalidFileNameChars());
                invalidChars.Add('™');
                invalidChars.Add('®');

                _newName = string.Join("", uncheckedName.Split(invalidChars.ToArray()));
                newPath = Path.GetDirectoryName(filepath) + "\\" + _newName;

                if (!File.Exists(newPath))
                {
                    System.IO.File.Move(filepath, (newPath + ".xci"));
                }
                else
                {
                    int append = 1;

                    while (File.Exists(newPath + "_" + append.ToString()))
                    {
                        append++;
                    }

                    newPath = newPath + "_" + append.ToString();

                    System.IO.File.Move(filepath, (newPath + ".xci"));
                }
            }
            return false;
        }
    }
}
