using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using static XCI_Organizer.Form1;
using XCI_Organizer.XTSSharp;

namespace XCI_Organizer {
    internal static class Util {
        public static string GetMkey(byte id) {
            switch (id) {
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
            }
            catch (System.Exception execpt) {
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
            switch (id) {
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

        public static bool RenameFile(string filepath, string newName) {
            // Needs to be updated
            if (checkFile(filepath)) {
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

                if (!File.Exists(newPath)) {
                    System.IO.File.Move(filepath, (newPath + ".xci"));

                }
                else {
                    int append = 1;

                    while (append < 5 && File.Exists(newPath + "_" + append.ToString())) {
                        append++;
                    }

                    newPath = newPath + "_" + append.ToString();

                    System.IO.File.Move(filepath, (newPath + ".xci"));
                }
            }
            return false;
        }

        public static byte[] DecryptNCAHeader(string selectedFile, long offset)
        {
            byte[] array = new byte[3072];
            if (File.Exists(selectedFile))
            {
                FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read);
                fileStream.Position = offset;
                fileStream.Read(array, 0, 3072);
                File.WriteAllBytes(selectedFile + ".tmp", array);
                Xts xts = XtsAes128.Create(Form1.NcaHeaderEncryptionKey1_Prod, Form1.NcaHeaderEncryptionKey2_Prod);
                using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(selectedFile + ".tmp")))
                {
                    using (XtsStream xtsStream = new XtsStream(binaryReader.BaseStream, xts, 512))
                    {
                        xtsStream.Read(array, 0, 3072);
                    }
                }
                File.Delete(selectedFile + ".tmp");
                fileStream.Close();
            }
            return array;
        }

        public static FileData GetFileData(string filepath)
        {
            FileData result = new FileData();
            //Basic Info
            result.FilePath = filepath;
            result.FileName = Path.GetFileNameWithoutExtension(filepath);
            result.FileNameWithExt = Path.GetFileName(filepath);

            if (CheckXCI(filepath))
            {
                //Get File Size
                string[] array_fs = new string[5] { "B", "KB", "MB", "GB", "TB" };
                double num_fs = (double)new FileInfo(filepath).Length;
                int num2_fs = 0;

                while (num_fs >= 1024.0 && num2_fs < array_fs.Length - 1)
                {
                    num2_fs++;
                    num_fs /= 1024.0;
                }
                result.ROMSize = $"{num_fs:0.##} {array_fs[num2_fs]}";

                double num3_fs = (double)(XCI.XCI_Headers[0].CardSize2 * 512 + 512);
                num2_fs = 0;
                while (num3_fs >= 1024.0 && num2_fs < array_fs.Length - 1)
                {
                    num2_fs++;
                    num3_fs /= 1024.0;
                }
                result.UsedSpace = $"{num3_fs:0.##} {array_fs[num2_fs]}";


                //Load Deep File Info (Probably we should clean it a bit more)
                string actualHash;
                byte[] hashBuffer;
                long offset;

                long[] SecureSize   = { } ;
                long[] NormalSize   = { } ;
                long[] SecureOffset = { };
                long[] NormalOffset = { };
                long gameNcaOffset  = -1;
                long gameNcaSize    = -1;
                long PFS0Offset     = -1;
                long PFS0Size       = -1;

                FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                HFS0.HSF0_Entry[] array = new HFS0.HSF0_Entry[HFS0.HFS0_Headers[0].FileCount];
                fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * HFS0.HFS0_Headers[0].FileCount;

                List<char> chars = new List<char>();
                long num = XCI.XCI_Headers[0].HFS0OffsetPartition + XCI.XCI_Headers[0].HFS0SizeParition;
                byte[] array2 = new byte[64];
                byte[] array3 = new byte[16];
                byte[] array4 = new byte[24];
                for (int i = 0; i < HFS0.HFS0_Headers[0].FileCount; i++)
                {
                    fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * i;
                    fileStream.Read(array2, 0, 64);
                    array[i] = new HFS0.HSF0_Entry(array2);
                    fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * HFS0.HFS0_Headers[0].FileCount + array[i].Name_ptr;
                    int num2;
                    while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0)
                    {
                        chars.Add((char)num2);
                    }
                    array[i].Name = new string(chars.ToArray());
                    chars.Clear();

                    offset = num + array[i].Offset;
                    hashBuffer = new byte[array[i].HashedRegionSize];
                    fileStream.Position = offset;
                    fileStream.Read(hashBuffer, 0, array[i].HashedRegionSize);
                    actualHash = SHA256Bytes(hashBuffer);

                    HFS0.HFS0_Header[] array5 = new HFS0.HFS0_Header[1];
                    fileStream.Position = array[i].Offset + num;
                    fileStream.Read(array3, 0, 16);
                    array5[0] = new HFS0.HFS0_Header(array3);
                    if (array[i].Name == "secure")
                    {
                        SecureSize = new long[array5[0].FileCount];
                        SecureOffset = new long[array5[0].FileCount];
                    }
                    if (array[i].Name == "normal")
                    {
                        NormalSize = new long[array5[0].FileCount];
                        NormalOffset = new long[array5[0].FileCount];
                    }
                    HFS0.HSF0_Entry[] array6 = new HFS0.HSF0_Entry[array5[0].FileCount];
                    for (int j = 0; j < array5[0].FileCount; j++)
                    {
                        fileStream.Position = array[i].Offset + num + 16 + 64 * j;
                        fileStream.Read(array2, 0, 64);
                        array6[j] = new HFS0.HSF0_Entry(array2);
                        fileStream.Position = array[i].Offset + num + 16 + 64 * array5[0].FileCount + array6[j].Name_ptr;
                        if (array[i].Name == "secure")
                        {
                            SecureSize[j] = array6[j].Size;
                            SecureOffset[j] = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                        }
                        if (array[i].Name == "normal")
                        {
                            NormalSize[j] = array6[j].Size;
                            NormalOffset[j] = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                        }
                        while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0)
                        {
                            chars.Add((char)num2);
                        }
                        array6[j].Name = new string(chars.ToArray());
                        chars.Clear();

                        offset = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                        hashBuffer = new byte[array6[j].HashedRegionSize];
                        fileStream.Position = offset;
                        fileStream.Read(hashBuffer, 0, array6[j].HashedRegionSize);
                        actualHash = SHA256Bytes(hashBuffer);
                    }
                }
                long num3 = -9223372036854775808L;
                for (int k = 0; k < SecureSize.Length; k++)
                {
                    if (SecureSize[k] > num3)
                    {
                        gameNcaSize = SecureSize[k];
                        gameNcaOffset = SecureOffset[k];
                        num3 = SecureSize[k];
                    }
                }
                PFS0Offset = gameNcaOffset + 32768;
                fileStream.Position = PFS0Offset;
                fileStream.Read(array3, 0, 16);
                PFS0.PFS0_Headers[0] = new PFS0.PFS0_Header(array3);
                PFS0.PFS0_Entry[] array8;
                array8 = new PFS0.PFS0_Entry[PFS0.PFS0_Headers[0].FileCount];
                for (int m = 0; m < PFS0.PFS0_Headers[0].FileCount; m++)
                {
                    fileStream.Position = PFS0Offset + 16 + 24 * m;
                    fileStream.Read(array4, 0, 24);
                    array8[m] = new PFS0.PFS0_Entry(array4);
                    PFS0Size += array8[m].Size;
                }
                for (int n = 0; n < PFS0.PFS0_Headers[0].FileCount; n++)
                {
                    fileStream.Position = PFS0Offset + 16 + 24 * PFS0.PFS0_Headers[0].FileCount + array8[n].Name_ptr;
                    int num4;
                    while ((num4 = fileStream.ReadByte()) != 0 && num4 != 0)
                    {
                        chars.Add((char)num4);
                    }
                    array8[n].Name = new string(chars.ToArray());
                    chars.Clear();
                }
                fileStream.Close();


                NCA.NCA_Headers[0] = new NCA.NCA_Header(DecryptNCAHeader(filepath, gameNcaOffset));
                result.TitleID = NCA.NCA_Headers[0].TitleID.ToString("X");
//                TB_TID.Text = "0" + NCA.NCA_Headers[0].TitleID.ToString("X");
                //TB_SDKVer.Text = $"{NCA.NCA_Headers[0].SDKVersion4}.{NCA.NCA_Headers[0].SDKVersion3}.{NCA.NCA_Headers[0].SDKVersion2}.{NCA.NCA_Headers[0].SDKVersion1}";
                //TB_MKeyRev.Text = Util.GetMkey(NCA.NCA_Headers[0].MasterKeyRev);






            }



            /*

                        if (CheckXCI(filepath)) {


                            long num = XCI.XCI_Headers[0].HFS0OffsetPartition + XCI.XCI_Headers[0].HFS0SizeParition;
                            byte[] array2 = new byte[64];
                            byte[] array3 = new byte[16];
                            byte[] array4 = new byte[24];
                            for (int i = 0; i < HFS0.HFS0_Headers[0].FileCount; i++)
                            {
                                fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * i;
                                fileStream.Read(array2, 0, 64);
                                array[i] = new HFS0.HSF0_Entry(array2);
                                fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * HFS0.HFS0_Headers[0].FileCount + array[i].Name_ptr;
                                int num2;
                                while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0)
                                {
                                    chars.Add((char)num2);
                                }
                                array[i].Name = new string(chars.ToArray());
                                chars.Clear();

                                offset = num + array[i].Offset;
                                hashBuffer = new byte[array[i].HashedRegionSize];
                                fileStream.Position = offset;
                                fileStream.Read(hashBuffer, 0, array[i].HashedRegionSize);
                                actualHash = SHA256Bytes(hashBuffer);

                                TV_Parti.AddFile(array[i].Name + ".hfs0", rootNode, offset, array[i].Size, array[i].HashedRegionSize, ByteArrayToString(array[i].Hash), actualHash);
                                BetterTreeNode betterTreeNode = TV_Parti.AddDir(array[i].Name, rootNode);
                                HFS0.HFS0_Header[] array5 = new HFS0.HFS0_Header[1];
                                fileStream.Position = array[i].Offset + num;
                                fileStream.Read(array3, 0, 16);
                                array5[0] = new HFS0.HFS0_Header(array3);
                                if (array[i].Name == "secure")
                                {
                                    SecureSize = new long[array5[0].FileCount];
                                    SecureOffset = new long[array5[0].FileCount];
                                }
                                if (array[i].Name == "normal")
                                {
                                    NormalSize = new long[array5[0].FileCount];
                                    NormalOffset = new long[array5[0].FileCount];
                                }
                                HFS0.HSF0_Entry[] array6 = new HFS0.HSF0_Entry[array5[0].FileCount];
                                for (int j = 0; j < array5[0].FileCount; j++)
                                {
                                    fileStream.Position = array[i].Offset + num + 16 + 64 * j;
                                    fileStream.Read(array2, 0, 64);
                                    array6[j] = new HFS0.HSF0_Entry(array2);
                                    fileStream.Position = array[i].Offset + num + 16 + 64 * array5[0].FileCount + array6[j].Name_ptr;
                                    if (array[i].Name == "secure")
                                    {
                                        SecureSize[j] = array6[j].Size;
                                        SecureOffset[j] = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                                    }
                                    if (array[i].Name == "normal")
                                    {
                                        NormalSize[j] = array6[j].Size;
                                        NormalOffset[j] = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                                    }
                                    while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0)
                                    {
                                        chars.Add((char)num2);
                                    }
                                    array6[j].Name = new string(chars.ToArray());
                                    chars.Clear();

                                    offset = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                                    hashBuffer = new byte[array6[j].HashedRegionSize];
                                    fileStream.Position = offset;
                                    fileStream.Read(hashBuffer, 0, array6[j].HashedRegionSize);
                                    actualHash = SHA256Bytes(hashBuffer);

                                    TV_Parti.AddFile(array6[j].Name, betterTreeNode, offset, array6[j].Size, array6[j].HashedRegionSize, ByteArrayToString(array6[j].Hash), actualHash);
                                    TreeNode[] array7 = TV_Partitions.Nodes.Find(betterTreeNode.Text, true);
                                    if (array7.Length != 0)
                                    {
                                        TV_Parti.AddFile(array6[j].Name, (BetterTreeNode)array7[0], 0L, 0L);
                                    }
                                }
                            }
                            long num3 = -9223372036854775808L;
                            for (int k = 0; k < SecureSize.Length; k++)
                            {
                                if (SecureSize[k] > num3)
                                {
                                    gameNcaSize = SecureSize[k];
                                    gameNcaOffset = SecureOffset[k];
                                    num3 = SecureSize[k];
                                }
                            }
                            PFS0Offset = gameNcaOffset + 32768;
                            fileStream.Position = PFS0Offset;
                            fileStream.Read(array3, 0, 16);
                            PFS0.PFS0_Headers[0] = new PFS0.PFS0_Header(array3);
                            PFS0.PFS0_Entry[] array8;
                            array8 = new PFS0.PFS0_Entry[PFS0.PFS0_Headers[0].FileCount];
                            for (int m = 0; m < PFS0.PFS0_Headers[0].FileCount; m++)
                            {
                                fileStream.Position = PFS0Offset + 16 + 24 * m;
                                fileStream.Read(array4, 0, 24);
                                array8[m] = new PFS0.PFS0_Entry(array4);
                                PFS0Size += array8[m].Size;
                            }
                            TV_Parti.AddFile("boot.psf0", rootNode, PFS0Offset, 16 + 24 * PFS0.PFS0_Headers[0].FileCount + 64 + PFS0Size);
                            BetterTreeNode betterTreeNode2 = TV_Parti.AddDir("boot", rootNode);
                            for (int n = 0; n < PFS0.PFS0_Headers[0].FileCount; n++)
                            {
                                fileStream.Position = PFS0Offset + 16 + 24 * PFS0.PFS0_Headers[0].FileCount + array8[n].Name_ptr;
                                int num4;
                                while ((num4 = fileStream.ReadByte()) != 0 && num4 != 0)
                                {
                                    chars.Add((char)num4);
                                }
                                array8[n].Name = new string(chars.ToArray());
                                chars.Clear();
                                TV_Parti.AddFile(array8[n].Name, betterTreeNode2, PFS0Offset + array8[n].Offset + 16 + PFS0.PFS0_Headers[0].StringTableSize + PFS0.PFS0_Headers[0].FileCount * 24, array8[n].Size);
                                TreeNode[] array9 = TV_Partitions.Nodes.Find(betterTreeNode2.Text, true);
                                if (array9.Length != 0)
                                {
                                    TV_Parti.AddFile(array8[n].Name, (BetterTreeNode)array9[0], 0L, 0L);
                                }
                            }
                            fileStream.Close();





                        }
            */

            return result;
        }
    }
}
