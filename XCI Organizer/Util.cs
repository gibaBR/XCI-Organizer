using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static XCI_Organizer.Form1;
using XCI_Organizer.XTSSharp;
using System.Runtime.InteropServices;
using System.Drawing;

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

        public static List<FileData> GetXCIsInFolder(string folder) {
            List<FileData> list = new List<FileData>();

            try {
                foreach (string f in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".xci") || s.ToLower().EndsWith(".nsp"))) {
                    FileData path = new FileData();
                    path.FilePath = f;
                    path.Header = GetXCIHeader(f);
                    if (Path.GetExtension(f).ToLower() == ".nsp") {
                        path.IsNSP = true;
                    }
                    list.Add(path);
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

        public static void RenameFile(string filepath, string newName) {
            // Needs to be updated
        }

        /* Switched to MemoryStream and simplified code
         * No idea if anything broke
         */
        public static byte[] DecryptNCAHeader(string selectedFile, long offset) {
            byte[] array = new byte[3072];
            if (File.Exists(selectedFile)) {
                using (var stream = new MemoryStream()) {
                    FileStream fs = new FileStream(selectedFile, FileMode.Open, FileAccess.Read);
                    fs.Position = offset;
                    fs.Read(array, 0, 3072);
                    fs.Close();

                    stream.Write(array, 0, array.Length);
                    stream.Position = 0;

                    Xts xts = XtsAes128.Create(Form1.NcaHeaderEncryptionKey1_Prod, Form1.NcaHeaderEncryptionKey2_Prod);
                    using (XtsStream xtsStream = new XtsStream(stream, xts, 512)) {
                        xtsStream.Read(array, 0, 3072);
                    }
                }
            }
            return array;
        }

        public static void GetFileSize(ref FileData file) {
            if (CheckXCI(file.FilePath)) {
                //Get File Size
                string[] array_fs = new string[5] { "B", "KB", "MB", "GB", "TB" };
                double num_fs = (double)new FileInfo(file.FilePath).Length;
                int num2_fs = 0;

                while (num_fs >= 1024.0 && num2_fs < array_fs.Length - 1) {
                    num2_fs++;
                    num_fs /= 1024.0;
                }
                file.ROMSize = $"{num_fs:0.##} {array_fs[num2_fs]}";

                double num3_fs = (double)(XCI.XCI_Headers[0].CardSize2 * 512 + 512);
                file.ExactUsedSpace = num3_fs.ToString();
                num2_fs = 0;
                while (num3_fs >= 1024.0 && num2_fs < array_fs.Length - 1) {
                    num2_fs++;
                    num3_fs /= 1024.0;
                }
                file.UsedSpace = $"{num3_fs:0.##} {array_fs[num2_fs]}";

                if (num_fs == num3_fs) {
                    file.Trimmed = "Yes";
                }
                else {
                    file.Trimmed = "No";
                }
            }
        }

        public static FileData GetFileData(string filepath) {
            FileData result = new FileData();

            if (CheckXCI(filepath)) {
                //Basic Info
                result.FilePath = filepath;
                result.FileName = Path.GetFileNameWithoutExtension(filepath);
                result.FileNameWithExt = Path.GetFileName(filepath);

                //Get File Size
                GetFileSize(ref result);

                //Load Deep File Info (Probably we should clean it a bit more)
                long[] SecureSize = { };
                long[] NormalSize = { };
                long[] SecureOffset = { };
                long[] NormalOffset = { };
                long gameNcaOffset = -1;

                FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                HFS0.HSF0_Entry[] array = new HFS0.HSF0_Entry[HFS0.HFS0_Headers[0].FileCount];
                fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * HFS0.HFS0_Headers[0].FileCount;

                List<char> chars = new List<char>();
                long num = XCI.XCI_Headers[0].HFS0OffsetPartition + XCI.XCI_Headers[0].HFS0SizeParition;
                byte[] array2 = new byte[64];
                byte[] array3 = new byte[16];
                byte[] array4 = new byte[24];
                for (int i = 0; i < HFS0.HFS0_Headers[0].FileCount; i++) {
                    fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * i;
                    fileStream.Read(array2, 0, 64);
                    array[i] = new HFS0.HSF0_Entry(array2);
                    fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * HFS0.HFS0_Headers[0].FileCount + array[i].Name_ptr;
                    int num2;
                    while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0) {
                        chars.Add((char)num2);
                    }
                    array[i].Name = new string(chars.ToArray());
                    chars.Clear();

                    HFS0.HFS0_Header[] array5 = new HFS0.HFS0_Header[1];
                    fileStream.Position = array[i].Offset + num;
                    fileStream.Read(array3, 0, 16);
                    array5[0] = new HFS0.HFS0_Header(array3);
                    if (array[i].Name == "secure") {
                        SecureSize = new long[array5[0].FileCount];
                        SecureOffset = new long[array5[0].FileCount];
                    }
                    HFS0.HSF0_Entry[] array6 = new HFS0.HSF0_Entry[array5[0].FileCount];
                    for (int j = 0; j < array5[0].FileCount; j++) {
                        fileStream.Position = array[i].Offset + num + 16 + 64 * j;
                        fileStream.Read(array2, 0, 64);
                        array6[j] = new HFS0.HSF0_Entry(array2);
                        fileStream.Position = array[i].Offset + num + 16 + 64 * array5[0].FileCount + array6[j].Name_ptr;
                        if (array[i].Name == "secure") {
                            SecureSize[j] = array6[j].Size;
                            SecureOffset[j] = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                        }
                        while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0) {
                            chars.Add((char)num2);
                        }
                        array6[j].Name = new string(chars.ToArray());
                        chars.Clear();
                    }
                }
                long num3 = -9223372036854775808L;
                for (int k = 0; k < SecureSize.Length; k++) {
                    if (SecureSize[k] > num3) {
                        gameNcaOffset = SecureOffset[k];
                        num3 = SecureSize[k];
                    }
                }
                fileStream.Close();

                NCA.NCA_Headers[0] = new NCA.NCA_Header(DecryptNCAHeader(filepath, gameNcaOffset));
                result.TitleID = "0" + NCA.NCA_Headers[0].TitleID.ToString("X");
            }
            return result;
        }

        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static T FromBinaryReader<T>(BinaryReader reader) {
            // Read in a byte array
            byte[] bytes = new byte[Marshal.SizeOf(typeof(T))];
            reader.Read(bytes, 0, Marshal.SizeOf(typeof(T)));

            // Pin the managed memory while, copy it out the data, then unpin it
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        public static xci_header GetXCIHeader(string inFile) {
            // TODO Check magic
            FileStream fs = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            xci_header header = FromBinaryReader<xci_header>(br);


            br.Close();
            fs.Close();
            return header;
        }

        public static bool ContainsUnicodeCharacter(string input) {
            const int MaxAnsiCode = 127;
            return input.Any(c => c > MaxAnsiCode);
        }
    }
}
