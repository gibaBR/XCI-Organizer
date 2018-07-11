using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XCI_Explorer;
using XCI_Organizer.Helpers;
using XCI_Organizer.XTSSharp;

namespace XCI_Organizer {
    public partial class Form1 : Form {
        private string[] Language = new string[16]
        {
            "American English",
            "British English",
            "Japanese",
            "French",
            "German",
            "Latin American Spanish",
            "Spanish",
            "Italian",
            "Dutch",
            "Canadian French",
            "Portuguese",
            "Russian",
            "Korean",
            "Taiwanese",
            "Chinese",
            "???"
        };

        public static byte[] NcaHeaderEncryptionKey1_Prod;
        public static byte[] NcaHeaderEncryptionKey2_Prod;
        public string Mkey;
        public string selectedFile;
        public double UsedSize;

        public IniFile ini;

        private Image[] Icons = new Image[16];
        private TreeViewFileSystem TV_Parti;
        private BetterTreeNode rootNode;
        public List<char> chars = new List<char>();
        List<FileData> files = new List<FileData>();
        string[] fileEntries;
        int sortByThis;

        private long[] SecureSize;
        private long[] NormalSize;
        private long[] SecureOffset;
        private long[] NormalOffset;
        private long gameNcaOffset;
        private long gameNcaSize;
        private long PFS0Offset;
        private long PFS0Size;
        private long selectedOffset;
        private long selectedSize;

        public class FileData {
            public string FilePath { get; set; } = "";
            public string FileName { get; set; } = "";
            public string FileNameWithExt { get; set; } = "";
            public string ROMSize { get; set; } = "";
            public string UsedSpace { get; set; } = "";
            public string TitleID { get; set; } = "";
            public string GameName { get; set; } = "";
            public string ReleaseID { get; set; } = "";
            public string Region { get; set; } = "";
            public string Languages { get; set; } = "";
        }

        public Form1() {
            InitializeComponent();

            // Set number of numbers in version number
            const int NUMBERSINVERSION = 3;

            B_CopyXCI.Visible = false;
            chUsedSpace.Width = 0;

            // Default sorting when program starts
            //sortByThis = chReleaseID.Index;

            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string[] versionArray = assemblyVersion.Split('.');
            assemblyVersion = string.Join(".", versionArray.Take(NUMBERSINVERSION));
            this.Text = "XCI Organizer v" + assemblyVersion + "rev1";
            bwUpdateFileList.WorkerReportsProgress = true;

            if (!File.Exists("keys.txt")) {
                if (MessageBox.Show("keys.txt is missing.\nDo you want to automatically download it now?\n\nBy pressing 'Yes' you agree that you own these keys.", "XCI Explorer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    using (var client = new WebClient()) {
                        string userKeys = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly9wYXN0ZWJpbi5jb20vcmF3L3BKTFJpUEZH"));
                        client.DownloadFile(@userKeys, "keys.txt");
                    }
                }

                if (!File.Exists("keys.txt")) {
                    MessageBox.Show("keys.txt failed to load.\nPlease include keys.txt in this location.");
                    Environment.Exit(0);
                }
            }

            if (!File.Exists("hactool.exe")) {
                MessageBox.Show("hactool.exe is missing.");
                Environment.Exit(0);
            }

            if (!File.Exists("db.xml")) {
                MessageBox.Show("NSWDB is missing.\nDownloading database...");
                updateNSWDB();
            }

            getKey();
            LoadSettings();
        }

        private void getKey() {
            string text = (from x in File.ReadAllLines("keys.txt")
                           select x.Split('=') into x
                           where x.Length > 1
                           select x).ToDictionary((string[] x) => x[0].Trim(), (string[] x) => x[1])["header_key"].Replace(" ", "");
            NcaHeaderEncryptionKey1_Prod = Util.StringToByteArray(text.Remove(32, 32));
            NcaHeaderEncryptionKey2_Prod = Util.StringToByteArray(text.Remove(0, 32));
        }


        public bool getMKey() {
            Dictionary<string, string> dictionary = (from x in File.ReadAllLines("keys.txt")
                                                     select x.Split('=') into x
                                                     where x.Length > 1
                                                     select x).ToDictionary((string[] x) => x[0].Trim(), (string[] x) => x[1]);
            Mkey = "master_key_";
            if (NCA.NCA_Headers[0].MasterKeyRev == 0 || NCA.NCA_Headers[0].MasterKeyRev == 1) {
                Mkey += "00";
            }
            else if (NCA.NCA_Headers[0].MasterKeyRev < 17) {
                int num = NCA.NCA_Headers[0].MasterKeyRev - 1;
                Mkey = Mkey + "0" + num.ToString();
            }
            else if (NCA.NCA_Headers[0].MasterKeyRev >= 17) {
                int num2 = NCA.NCA_Headers[0].MasterKeyRev - 1;
                Mkey += num2.ToString();
            }

            try {
                Mkey = dictionary[Mkey].Replace(" ", "");
                return true;
            }
            catch {
                return false;
            }
        }

        private void btnBaseFolder_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderFileDialog = new FolderBrowserDialog();
            folderFileDialog.Description = "Select the base folder for your collection:";
            if (folderFileDialog.ShowDialog() == DialogResult.OK) {
                ini.IniWriteValue("Config", "BaseFolder", folderFileDialog.SelectedPath);
                ini.IniWriteValue("Config", "DefaultSort", "0");
                UpdateFileList();
            }
        }

        private void UpdateFileList() {
            string selectedPath = ini.IniReadValue("Config", "BaseFolder");

            sortByThis = int.Parse(ini.IniReadValue("Config", "DefaultSort", "0"));

            contextMenuStrip1.Enabled = false;

            if (Directory.Exists(selectedPath) && selectedPath.Trim() != "") {
                txbBaseFolder.Text = selectedPath;
                LV_Files.Items.Clear();

                string[] directories = Directory.GetDirectories(selectedPath);
                fileEntries = Directory.GetFiles(selectedPath, "*.xci", SearchOption.AllDirectories);
                files = Util.GetXCIsInFolder(selectedPath);

                if (!bwUpdateFileList.IsBusy) {
                    buttonsEnabled(false);

                    // Start the asynchronous operation.
                    L_Status.Text = "Status: Refreshing list...";
                    bwUpdateFileList.RunWorkerAsync();
                }
            }
        }

        private void ProcessFile() {
            if (Util.CheckXCI(selectedFile)) {
                LoadXCI();
            }
            else {
                // TB_File.Text = null;
                MessageBox.Show("Unsupported file.");
            }
        }

        private void _TrimXCI() {
            FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Write);
            fileStream.SetLength((long)UsedSize);
            fileStream.Close();
        }

        private void TrimXCI() {
            if (!Util.checkFile(selectedFile)) {
                MessageBox.Show("File not found");
                return;
            }
            if (TB_ROMExactSize.Text.Equals(TB_ExactUsedSpace.Text)) {
                MessageBox.Show("No trimming needed!");
                return;
            }
            if (MessageBox.Show("Trim XCI?", "XCI Organizer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                _TrimXCI();
                MessageBox.Show("Done.");
                string[] array = new string[5]
                {
                    "B",
                    "KB",
                    "MB",
                    "GB",
                    "TB"
                };
                double num = (double)new FileInfo(selectedFile).Length;
                TB_ROMExactSize.Text = "(" + num.ToString() + " bytes)";
                int num2 = 0;
                while (num >= 1024.0 && num2 < array.Length - 1) {
                    num2++;
                    num /= 1024.0;
                }
                TB_ROMSize.Text = $"{num:0.##} {array[num2]}";
                double num3 = UsedSize = (double)(XCI.XCI_Headers[0].CardSize2 * 512 + 512);
                TB_ExactUsedSpace.Text = "(" + num3.ToString() + " bytes)";
                num2 = 0;
                while (num3 >= 1024.0 && num2 < array.Length - 1) {
                    num2++;
                    num3 /= 1024.0;
                }
                TB_UsedSpace.Text = $"{num3:0.##} {array[num2]}";
            }
        }

        private void LoadXCI() {
            string[] array = new string[5]
            {
                "B",
                "KB",
                "MB",
                "GB",
                "TB"
            };

            double num = (double)new FileInfo(selectedFile).Length;
            TB_ROMExactSize.Text = "(" + num.ToString() + " bytes)";
            int num2 = 0;

            while (num >= 1024.0 && num2 < array.Length - 1) {
                num2++;
                num /= 1024.0;
            }

            TB_ROMSize.Text = $"{num:0.##} {array[num2]}";
            double num3 = UsedSize = (double)(XCI.XCI_Headers[0].CardSize2 * 512 + 512);
            TB_ExactUsedSpace.Text = "(" + num3.ToString() + " bytes)";
            num2 = 0;

            while (num3 >= 1024.0 && num2 < array.Length - 1) {
                num2++;
                num3 /= 1024.0;
            }

            TB_UsedSpace.Text = $"{num3:0.##} {array[num2]}";
            TB_Capacity.Text = Util.GetCapacity(XCI.XCI_Headers[0].CardSize1);
            LoadPartitons();
            LoadNCAData();
            LoadGameInfos();
        }

        private void LoadGameInfos() {
            CB_RegionName.Items.Clear();
            CB_RegionName.Enabled = true;
            TB_Name.Text = "";
            TB_Dev.Text = "";
            PB_GameIcon.BackgroundImage = null;
            Array.Clear(Icons, 0, Icons.Length);
            if (getMKey()) {
                using (FileStream fileStream = File.OpenRead(selectedFile)) {
                    for (int si = 0; si < SecureSize.Length; si++) {
                        if (SecureSize[si] > 0x4E20000) continue;

                        if (File.Exists("meta")) {
                            File.Delete("meta");
                        }

                        if (Directory.Exists("data")) {
                            Directory.Delete("data", true);
                        }

                        using (FileStream fileStream2 = File.OpenWrite("meta")) {
                            fileStream.Position = SecureOffset[si];
                            byte[] buffer = new byte[8192];
                            long num = SecureSize[si];
                            int num2;
                            while ((num2 = fileStream.Read(buffer, 0, 8192)) > 0 && num > 0) {
                                fileStream2.Write(buffer, 0, num2);
                                num -= num2;
                            }
                            fileStream2.Close();
                        }

                        Process process = new Process();
                        process.StartInfo = new ProcessStartInfo {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "hactool.exe",
                            Arguments = "-k keys.txt --romfsdir=data meta"
                        };
                        process.Start();
                        process.WaitForExit();

                        if (File.Exists("data\\control.nacp")) {
                            byte[] source = File.ReadAllBytes("data\\control.nacp");
                            NACP.NACP_Datas[0] = new NACP.NACP_Data(source.Skip(0x3000).Take(0x1000).ToArray());
                            for (int i = 0; i < NACP.NACP_Strings.Length; i++) {
                                NACP.NACP_Strings[i] = new NACP.NACP_String(source.Skip(i * 0x300).Take(0x300).ToArray());
                                if (NACP.NACP_Strings[i].Check != 0) {
                                    CB_RegionName.Items.Add(Language[i]);
                                    string icon_filename = "data\\icon_" + Language[i].Replace(" ", "") + ".dat";
                                    if (File.Exists(icon_filename)) {
                                        using (Bitmap original = new Bitmap(icon_filename)) {
                                            Icons[i] = new Bitmap(original);
                                            PB_GameIcon.BackgroundImage = Icons[i];
                                        }
                                    }
                                }
                            }

                            TB_GameRev.Text = NACP.NACP_Datas[0].GameVer;
                            TB_ProdCode.Text = NACP.NACP_Datas[0].GameProd;

                            if (TB_ProdCode.Text == "") {
                                TB_ProdCode.Text = "No Prod. ID";
                            }

                            try {
                                File.Delete("meta");
                                Directory.Delete("data", true);
                            }
                            catch { }

                            CB_RegionName.SelectedIndex = 0;
                            break;
                        }
                    }
                    fileStream.Close();
                }
            }
            else {
                TB_Dev.Text = Mkey + " not found";
                TB_Name.Text = Mkey + " not found";
            }
        }

        private void LoadSettings() {
            ini = new IniFile((AppDomain.CurrentDomain.BaseDirectory) + "XCI_Organizer.ini");
            UpdateFileList();
            //ini.IniReadValue("Config", "BaseFolder");
            R_BatchRenameCustomText.Text = ini.IniReadValue("Config", "RenameCustomText", "%ID% - %NAME% (%REGION%)");
        }

        private void ClearFields() {
            CB_RegionName.Items.Clear();
            TB_Name.Text = "";
            TB_Dev.Text = "";
            TB_TID.Text = "";
            TB_GameRev.Text = "";
            TB_ProdCode.Text = "";
            TB_SDKVer.Text = "";
            TB_MKeyRev.Text = "";
            TB_Capacity.Text = "";
            TB_ROMSize.Text = "";
            TB_ROMExactSize.Text = "";
            TB_UsedSpace.Text = "";
            TB_ExactUsedSpace.Text = "";
            PB_GameIcon.BackgroundImage = null;
            TV_Partitions.Nodes.Clear();

            LB_SelectedData.Text = "";
            LB_DataOffset.Text = "";
            LB_DataSize.Text = "";
            LB_HashedRegionSize.Text = "";
            LB_ActualHash.Text = "";
            LB_ExpectedHash.Text = "";
        }

        private void LoadNCAData() {
            NCA.NCA_Headers[0] = new NCA.NCA_Header(Util.DecryptNCAHeader(selectedFile, gameNcaOffset));
            TB_TID.Text = "0" + NCA.NCA_Headers[0].TitleID.ToString("X");
            TB_SDKVer.Text = $"{NCA.NCA_Headers[0].SDKVersion4}.{NCA.NCA_Headers[0].SDKVersion3}.{NCA.NCA_Headers[0].SDKVersion2}.{NCA.NCA_Headers[0].SDKVersion1}";
            TB_MKeyRev.Text = Util.GetMkey(NCA.NCA_Headers[0].MasterKeyRev);
        }

        public static string ByteArrayToString(byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length * 2 + 2);
            hex.Append("0x");
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string SHA256Bytes(byte[] ba) {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] hashValue;
            hashValue = mySHA256.ComputeHash(ba);
            return ByteArrayToString(hashValue);
        }

        private void LoadPartitons() {
            string actualHash;
            byte[] hashBuffer;
            long offset;

            TV_Partitions.Nodes.Clear();
            TV_Parti = new TreeViewFileSystem(TV_Partitions);
            rootNode = new BetterTreeNode("root");
            rootNode.Offset = -1L;
            rootNode.Size = -1L;
            TV_Partitions.Nodes.Add(rootNode);
            FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read);
            HFS0.HSF0_Entry[] array = new HFS0.HSF0_Entry[HFS0.HFS0_Headers[0].FileCount];
            fileStream.Position = XCI.XCI_Headers[0].HFS0OffsetPartition + 16 + 64 * HFS0.HFS0_Headers[0].FileCount;
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
                if (array[i].Name == "secure") {
                    SecureSize = new long[array5[0].FileCount];
                    SecureOffset = new long[array5[0].FileCount];
                }
                if (array[i].Name == "normal") {
                    NormalSize = new long[array5[0].FileCount];
                    NormalOffset = new long[array5[0].FileCount];
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
                    if (array[i].Name == "normal") {
                        NormalSize[j] = array6[j].Size;
                        NormalOffset[j] = array[i].Offset + array6[j].Offset + num + 16 + array5[0].StringTableSize + array5[0].FileCount * 64;
                    }
                    while ((num2 = fileStream.ReadByte()) != 0 && num2 != 0) {
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
                    if (array7.Length != 0) {
                        TV_Parti.AddFile(array6[j].Name, (BetterTreeNode)array7[0], 0L, 0L);
                    }
                }
            }
            long num3 = -9223372036854775808L;
            for (int k = 0; k < SecureSize.Length; k++) {
                if (SecureSize[k] > num3) {
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
            for (int m = 0; m < PFS0.PFS0_Headers[0].FileCount; m++) {
                fileStream.Position = PFS0Offset + 16 + 24 * m;
                fileStream.Read(array4, 0, 24);
                array8[m] = new PFS0.PFS0_Entry(array4);
                PFS0Size += array8[m].Size;
            }
            TV_Parti.AddFile("boot.psf0", rootNode, PFS0Offset, 16 + 24 * PFS0.PFS0_Headers[0].FileCount + 64 + PFS0Size);
            BetterTreeNode betterTreeNode2 = TV_Parti.AddDir("boot", rootNode);
            for (int n = 0; n < PFS0.PFS0_Headers[0].FileCount; n++) {
                fileStream.Position = PFS0Offset + 16 + 24 * PFS0.PFS0_Headers[0].FileCount + array8[n].Name_ptr;
                int num4;
                while ((num4 = fileStream.ReadByte()) != 0 && num4 != 0) {
                    chars.Add((char)num4);
                }
                array8[n].Name = new string(chars.ToArray());
                chars.Clear();
                TV_Parti.AddFile(array8[n].Name, betterTreeNode2, PFS0Offset + array8[n].Offset + 16 + PFS0.PFS0_Headers[0].StringTableSize + PFS0.PFS0_Headers[0].FileCount * 24, array8[n].Size);
                TreeNode[] array9 = TV_Partitions.Nodes.Find(betterTreeNode2.Text, true);
                if (array9.Length != 0) {
                    TV_Parti.AddFile(array8[n].Name, (BetterTreeNode)array9[0], 0L, 0L);
                }
            }
            fileStream.Close();
        }

        private void CB_RegionName_SelectedIndexChanged(object sender, EventArgs e) {
            int num = Array.FindIndex(Language, (string element) => element.StartsWith(CB_RegionName.Text, StringComparison.Ordinal));
            PB_GameIcon.BackgroundImage = Icons[num];
            TB_Name.Text = NACP.NACP_Strings[num].GameName;
            TB_Dev.Text = NACP.NACP_Strings[num].GameAuthor;
        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e) {
            // opens the folder in explorer
            if ((LV_Files.Items.Count > 0) && (LV_Files.SelectedItems.Count >= 0)) {
                System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(selectedFile));
            }
        }

        private void trimXCIToolStripMenuItem_Click(object sender, EventArgs e) {
            TrimXCI();
        }

        private void B_TrimXCI_Click(object sender, EventArgs e) {
            TrimXCI();
        }

        private void BT_Refresh_Click(object sender, EventArgs e) {
            UpdateFileList();
        }

        private void B_ExportCert_Click(object sender, EventArgs e) {
            if (Util.checkFile(selectedFile)) {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "gamecard_cert.dat (*.dat)|*.dat";
                saveFileDialog.FileName = Path.GetFileName("gamecard_cert.dat");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read);
                    byte[] array = new byte[512];
                    fileStream.Position = 28672L;
                    fileStream.Read(array, 0, 512);
                    File.WriteAllBytes(saveFileDialog.FileName, array);
                    fileStream.Close();
                    MessageBox.Show("cert successfully exported to:\n\n" + saveFileDialog.FileName);
                }
            }
            else {
                MessageBox.Show("File not found");
            }
        }

        private void B_ImportCert_Click(object sender, EventArgs e) {
            if (Util.checkFile(selectedFile)) {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "gamecard_cert (*.dat)|*.dat|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK && new FileInfo(openFileDialog.FileName).Length == 512) {
                    using (Stream stream = File.Open(selectedFile, FileMode.Open)) {
                        stream.Position = 28672L;
                        stream.Write(File.ReadAllBytes(openFileDialog.FileName), 0, 512);
                    }
                    MessageBox.Show("cert successfully imported from:\n\n" + openFileDialog.FileName);
                }
            }
            else {
                MessageBox.Show("File not found");
            }
        }

        private void B_ViewCert_Click(object sender, EventArgs e) {
            if (Util.checkFile(selectedFile)) {
                new CertForm(this).Show();
            }
            else {
                MessageBox.Show("File not found");
            }
        }

        private void B_CopyXCI_Click(object sender, EventArgs e) {
            SendFileToSD();
        }

        private void SendFileToSD() {
            MessageBox.Show("Soon®");
        }

        private void sendToSDCardToolStripMenuItem_Click(object sender, EventArgs e) {
            SendFileToSD();
        }

        private void B_ClearCert_Click(object sender, EventArgs e) {
            if (Util.checkFile(selectedFile)) {
                if (MessageBox.Show("The cert will be deleted permanently.\nContinue?", "XCI Explorer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    using (Stream stream = File.Open(selectedFile, FileMode.Open)) {
                        byte[] array = new byte[512];
                        for (int i = 0; i < array.Length; i++) {
                            array[i] = byte.MaxValue;
                        }
                        stream.Position = 28672L;
                        stream.Write(array, 0, array.Length);
                        MessageBox.Show("cert deleted.");
                    }
                }
            }
            else {
                MessageBox.Show("File not found");
            }

        }

        private void TV_Partitions_AfterSelect(object sender, TreeViewEventArgs e) {
            BetterTreeNode betterTreeNode = (BetterTreeNode)TV_Partitions.SelectedNode;
            if (betterTreeNode.Offset != -1) {
                selectedOffset = betterTreeNode.Offset;
                selectedSize = betterTreeNode.Size;
                string expectedHash = betterTreeNode.ExpectedHash;
                string actualHash = betterTreeNode.ActualHash;
                long HashedRegionSize = betterTreeNode.HashedRegionSize;

                LB_DataOffset.Text = "Offset: 0x" + selectedOffset.ToString("X");
                LB_SelectedData.Text = e.Node.Text;
                if (!backgroundWorker1.IsBusy && !bwUpdateFileList.IsBusy) {
                    B_Extract.Enabled = true;
                }
                string[] array = new string[5]
                {
                    "B",
                    "KB",
                    "MB",
                    "GB",
                    "TB"
                };
                double num = (double)selectedSize;
                int num2 = 0;
                while (num >= 1024.0 && num2 < array.Length - 1) {
                    num2++;
                    num /= 1024.0;
                }
                LB_DataSize.Text = "Size:   0x" + selectedSize.ToString("X") + " (" + num.ToString() + array[num2] + ")";

                if (HashedRegionSize != 0) {
                    LB_HashedRegionSize.Text = "HashedRegionSize: 0x" + HashedRegionSize.ToString("X");
                }
                else {
                    LB_HashedRegionSize.Text = "";
                }

                if (!string.IsNullOrEmpty(expectedHash)) {
                    LB_ExpectedHash.Text = "Header Hash: " + expectedHash.Substring(0, 32);
                }
                else {
                    LB_ExpectedHash.Text = "";
                }

                if (!string.IsNullOrEmpty(actualHash)) {
                    LB_ActualHash.Text = "Actual Hash: " + actualHash.Substring(0, 32);
                    if (actualHash == expectedHash) {
                        LB_ActualHash.ForeColor = System.Drawing.Color.Green;
                    }
                    else {
                        LB_ActualHash.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else {
                    LB_ActualHash.Text = "";
                }

            }
            else {
                LB_SelectedData.Text = "";
                LB_DataOffset.Text = "";
                LB_DataSize.Text = "";
                LB_HashedRegionSize.Text = "";
                LB_ExpectedHash.Text = "";
                LB_ActualHash.Text = "";
                B_Extract.Enabled = false;
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            string fileName = (string)e.Argument;

            using (FileStream fileStream = File.OpenRead(selectedFile)) {
                using (FileStream fileStream2 = File.OpenWrite(fileName)) {
                    new BinaryReader(fileStream);
                    new BinaryWriter(fileStream2);
                    fileStream.Position = selectedOffset;
                    byte[] buffer = new byte[8192];
                    long num = selectedSize;
                    int num2;
                    while ((num2 = fileStream.Read(buffer, 0, 8192)) > 0 && num > 0) {
                        fileStream2.Write(buffer, 0, num2);
                        num -= num2;
                    }
                    fileStream.Close();
                }
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            buttonsEnabled(true);

            if (e.Error != null) {
                L_Status.Text = "Status: Error extracting NCA...";
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else {
                L_Status.Text = "Status: Done extracting NCA!";
            }
        }

        private void B_Extract_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = LB_SelectedData.Text;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                if (!backgroundWorker1.IsBusy) {
                    buttonsEnabled(false);

                    L_Status.Text = "Status: Extracting NCA...";

                    // Start the asynchronous operation.
                    backgroundWorker1.RunWorkerAsync(saveFileDialog.FileName);
                }
            }
        }

        private void R_BatchRenameCustomText_Enter(object sender, EventArgs e) {
            // Check Custom Rename on focus of textbox
            R_BatchRenameCustom.Checked = true;

            // Show tooltip with options on focus
            TextBox tb = (TextBox)sender;
            ToolTip tt = new ToolTip();
            int VisibleTime = 7000;
            tt.Show("Options:\n%ID%\n%NAME%\n%PUBLISHER%\n%GROUP%\n%REGION%\n%LANGUAGES%\n%SERIAL%\n%TITLEID%\n%RELEASENAME%\n%FIRMWARE%", tb, 0, 20, VisibleTime);
        }

        private bool IsGamesListUpToDate() {
            string selectedPath = ini.IniReadValue("Config", "BaseFolder");
            string[] currentFileEntries = Directory.GetFiles(selectedPath, "*.xci", SearchOption.AllDirectories);
            return String.Join(", ", fileEntries) == String.Join(", ", currentFileEntries);
        }

        private void BT_BatchRename_Click(object sender, EventArgs e) {
            // Added back into main function because I was trying to debug it. Needs to be added into Util again
            string selectedPath = ini.IniReadValue("Config", "BaseFolder");

            if (!IsGamesListUpToDate()) {
                MessageBox.Show("Games directory has changed, please refresh and try again.", "XCI Organizer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedPath.Trim() != "" && MessageBox.Show("Are you sure you want to rename ALL of your XCI files automatically?", "XCI Organizer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                L_Status.Text = "Status: Batch renaming files...";
                buttonsEnabled(false);
                /* Blake's version of trying not to get confused (tm)
                 * 
                 * "files" looks like "P:/ath/random.xci"
                 * "renamedFiles" looks like "FixedRandom"
                 * Duplicates look like "FixedRandom_yy.."
                 * 
                 * We need renamedFiles to just be the filename because some files with same name are located in different folders
                */
                List<string> renamedFiles = new List<string>(); // Add renamed files to new list so duplicates aren't created
                List<char> invalidChars = new List<char>(); // Custom list of invalid characters

                // Add characters to remove from filename here
                invalidChars.AddRange(Path.GetInvalidFileNameChars());
                invalidChars.Add('™');
                invalidChars.Add('®');

                foreach (FileData file in files) {
                    string uncheckedName = TB_Name.Text.ToString();
                    string checkedName;
                    string newPath;

                    /* Check NSWDB for filenames first
                     * If no entry, use custom naming scheme
                     */
                    XmlDocument doc = new XmlDocument();
                    doc.Load("db.xml");
                    var nodePath = "releases/release[titleid = '" + file.TitleID + "']";
                    var node = doc.SelectSingleNode(nodePath);
                    if (node != null) {
                        var id = node["id"].InnerText;
                        var name = node["name"].InnerText;
                        var publisher = node["publisher"].InnerText;
                        var group = node["group"].InnerText;
                        var region = node["region"].InnerText;
                        var languages = node["languages"].InnerText;
                        var serial = node["serial"].InnerText;
                        var titleid = node["titleid"].InnerText;
                        var releaseName = node["releasename"].InnerText;
                        var firmware = node["firmware"].InnerText;
                        string nameScheme;

                        // Change region to something more human
                        if (region == "WLD") {
                            region = "World";
                        }
                        else if (region == "EUR") {
                            region = "Europe";
                        }
                        else if (region == "JPN") {
                            region = "Japan";
                        }
                        else if (region == "KOR") {
                            region = "Korea";
                        }
                        else if (region == "SPA") {
                            region = "Spain";
                        }

                        if (R_BatchRenameSimple.Checked) {
                            nameScheme = name + " (" + region + ")";
                        }
                        else if (R_BatchRenameDetailed.Checked) {
                            nameScheme = id.ToString().PadLeft(4, '0') + " - " + name + " (" + region + ") (" + languages + ")";
                        }
                        else if (R_BatchRenameScene.Checked) {
                            nameScheme = releaseName;
                        }
                        else {
                            ini.IniWriteValue("Config", "RenameCustomText", R_BatchRenameCustomText.Text);

                            nameScheme = R_BatchRenameCustomText.Text
                                .Replace("%ID%", id.ToString().PadLeft(4, '0'))
                              .Replace("%NAME%", name)
                               .Replace("%PUBLISHER%", publisher)
                               .Replace("%GROUP%", group)
                               .Replace("%REGION%", region)
                               .Replace("%LANGUAGES%", languages)
                               .Replace("%SERIAL%", serial)
                               .Replace("%TITLEID%", titleid)
                               .Replace("%RELEASENAME%", releaseName)
                               .Replace("%FIRMWARE%", firmware);
                        }

                        checkedName = string.Join("", nameScheme.Split(invalidChars.ToArray())); ;
                    }
                    else {
                        checkedName = string.Join("", uncheckedName.Split(invalidChars.ToArray()));
                    }

                    newPath = Path.GetDirectoryName(file.FilePath) + "\\" + checkedName;

                    if (!renamedFiles.Contains(checkedName) && File.Exists(file.FilePath) && !File.Exists(newPath)) {
                        System.IO.File.Move(file.FilePath, (newPath + ".xci"));
                        renamedFiles.Add(checkedName);
                    }
                    else {
                        /* This will rename duplicate XCI and append "_DATESCHEME"
                         */
                        // Check if the file has already been renamed according to date naming scheme
                        bool alreadyRenamed = file.FilePath.Contains(newPath + "_" + DateTime.Now.ToString("yy"));

                        // Build new path using the date naming scheme
                        newPath = newPath + "_" + DateTime.Now.ToString("yyMMddHHmmssffffff");

                        // Only rename the file if it hasn't been renamed according to date naming scheme
                        if (!File.Exists(newPath) && !alreadyRenamed) {
                            try {
                                System.IO.File.Move(file.FilePath, (newPath + ".xci"));
                            }
                            catch { }
                        }
                    }
                }
                UpdateFileList();
                buttonsEnabled(true);
                L_Status.Text = "Status: Done batch renaming files!";
            }
        }

        private void BT_BatchTrim_Click(object sender, EventArgs e) {
            // This should be safe now because it uses the same files list from UpdateFileList
            string selectedPath = ini.IniReadValue("Config", "BaseFolder");

            if (!IsGamesListUpToDate()) {
                MessageBox.Show("Games directory has changed, please refresh and try again.", "XCI Organizer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedPath.Trim() != "" && MessageBox.Show("Are you sure you want to trim ALL of your XCI files automatically?\n", "XCI Organizer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                L_Status.Text = "Status: Batch trimming...";
                buttonsEnabled(false);

                foreach (FileData file in files) {
                    if (!file.ROMSize.Equals(file.UsedSpace) && File.Exists(file.FilePath)) {
                        _TrimXCI();
                    }
                }
                // Info will be outdated if something is trimmed :/ Need better solution
                if (File.Exists("cache.dat")) {
                    File.Delete("cache.dat");
                }
                UpdateFileList();
                buttonsEnabled(true);
                L_Status.Text = "Status: Batch trimming done!";
            }
        }

        private void autoRenameFileToolStripMenuItem_Click(object sender, EventArgs e) {
            // Util function needs to be refactored
            MessageBox.Show("Soon®");
        }

        private void PB_GameIcon_Click(object sender, EventArgs e) {
            // This needs a lot more work and probably will depend on code from the renamer once it's finished
            if (MessageBox.Show("Are you sure you want to save the current icon image?\n", "XCI Organizer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                int num = Array.FindIndex(Language, (string element) => element.StartsWith(CB_RegionName.Text, StringComparison.Ordinal));
                string iconFile = "icon_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff") + ".jpg";

                Bitmap copy = new Bitmap(Icons[num]);
                copy.Save(iconFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void updateNSWDB() {
            using (var client = new WebClient()) {
                client.DownloadFile(@"http://nswdb.com/xml.php", "db.xml");
            }
        }

        private void B_UpdateNSWDB_Click(object sender, EventArgs e) {
            buttonsEnabled(false);
            updateNSWDB();
            MessageBox.Show("Updated NSWDB!");
            buttonsEnabled(true);
        }

        private void LV_Files_SelectedIndexChanged(object sender, EventArgs e) {
            ListView.SelectedListViewItemCollection selection = LV_Files.SelectedItems;
            foreach (ListViewItem item in selection) {
                ClearFields();
                selectedFile = item.SubItems[chFilePath.Index].Text;
                if (selectedFile.Trim() != "") {
                    ProcessFile();
                }

                break; //Only First Item!
            }
        }

        private void buttonsEnabled(bool status) {
            btnBaseFolder.Enabled = status;
            BT_Refresh.Enabled = status;
            B_TrimXCI.Enabled = status;
            B_CopyXCI.Enabled = status;
            B_ExportCert.Enabled = status;
            B_ImportCert.Enabled = status;
            B_ViewCert.Enabled = status;
            B_ClearCert.Enabled = status;
            B_Extract.Enabled = status;
            BT_BatchRename.Enabled = status;
            BT_BatchTrim.Enabled = status;
            B_UpdateNSWDB.Enabled = status;
        }

        private void bwUpdateFileList_DoWork(object sender, DoWorkEventArgs e) {
            int counter = 0, percent;
            XmlDocument doc = new XmlDocument();
            XmlDocument cacheDoc = new XmlDocument();

            if (!File.Exists("cache.dat")) {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("    ");
                using (XmlWriter writer = XmlWriter.Create("cache.dat", settings)) {
                    // Setup file
                    writer.WriteStartElement("xciorganizer");
                    writer.WriteStartElement("pathpair");
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            else {
                // Check if necessary to run every update
                removeOldCacheEntries();

                cacheDoc.Load("cache.dat");
                foreach (FileData file in files) {
                    var cacheNodePath = "xciorganizer/pathpair[path = '" + file.FilePath + "']";
                    var cacheNode = cacheDoc.SelectSingleNode(cacheNodePath);
                    var titleid = "";
                    var romsize = "";
                    var usedspace = "";

                    if (cacheNode != null) {
                        titleid = cacheNode["titleid"].InnerText;
                        romsize = cacheNode["romsize"].InnerText;
                        usedspace = cacheNode["usedspace"].InnerText;
                    }

                    file.TitleID = titleid;
                    file.ROMSize = romsize;
                    file.UsedSpace = usedspace;
                }
            }

            doc.Load("db.xml");
            foreach (FileData file in files) {
                FileData data = new FileData();

                cacheDoc.Load("cache.dat");

                if (file.TitleID == "") {
                    data = Util.GetFileData(file.FilePath);
                    XDocument xDocument = XDocument.Load("cache.dat");
                    XElement root = xDocument.Element("xciorganizer");
                    IEnumerable<XElement> rows = root.Descendants("pathpair");
                    XElement firstRow = rows.First();
                    firstRow.AddBeforeSelf(
                       new XElement("pathpair",
                       new XElement("path", file.FilePath),
                       new XElement("titleid", data.TitleID),
                       new XElement("romsize", data.ROMSize),
                       new XElement("usedspace", data.UsedSpace)));

                    xDocument.Save("cache.dat");
                    Debug.WriteLine(file.FilePath + " written to cache");
                }
                else {
                    data.TitleID = file.TitleID;
                    data.ROMSize = file.ROMSize;
                    data.UsedSpace = file.UsedSpace;
                    Debug.WriteLine(file.FilePath + " read from cache");
                }

                // Fix TitleID
                data.TitleID = "0" + data.TitleID;

                var nodePath = "releases/release[titleid = '" + data.TitleID + "']";
                var node = doc.SelectSingleNode(nodePath);
                var releaseid = "";
                var region = "";
                var languages = "";
                var gamename = "";

                if (node != null) {
                    releaseid = node["id"].InnerText;
                    region = node["region"].InnerText;
                    languages = node["languages"].InnerText;
                    gamename = node["name"].InnerText;

                    // Change region to something more human
                    if (region == "WLD") {
                        region = "World";
                    }
                    else if (region == "EUR") {
                        region = "Europe";
                    }
                    else if (region == "JPN") {
                        region = "Japan";
                    }
                    else if (region == "KOR") {
                        region = "Korea";
                    }
                    else if (region == "SPA") {
                        region = "Spain";
                    }
                }

                // If can't find in db.xml
                if (gamename.Trim() == "") {
                    data.GameName = Path.GetFileNameWithoutExtension(file.FilePath);
                    data.ReleaseID = "0";
                    data.Region = "???";
                    data.Languages = "???";
                }
                else {
                    data.GameName = gamename;
                    data.ReleaseID = releaseid;
                    data.Region = region;
                    data.Languages = languages;
                }

                file.FileName = data.FileName;
                file.FileNameWithExt = data.FileNameWithExt;
                file.ROMSize = data.ROMSize;
                file.UsedSpace = data.UsedSpace;
                file.TitleID = data.TitleID;
                file.GameName = data.GameName;
                file.ReleaseID = data.ReleaseID;
                file.Region = data.Region;
                file.Languages = data.Languages;
                //Debug.WriteLine(file.ReleaseID);

                percent = (int)(++counter / (float)files.Count * 100);
                bwUpdateFileList.ReportProgress(percent, null);
            }

            // Not sure about preformance
            if (sortByThis == chReleaseID.Index) {
                files = files.OrderBy(a => int.Parse(a.ReleaseID)).ToList();
            }
            else {
                files = files.OrderBy(a => a.GameName).ToList();
            }

            counter = 0;
            foreach (FileData file in files) {
                ListViewItem item = new ListViewItem();
                item.Text = file.ReleaseID.PadLeft(4, '0');
                item.SubItems.Add(file.GameName);
                item.SubItems.Add(file.Region);
                item.SubItems.Add(file.Languages);
                item.SubItems.Add(file.TitleID);
                item.SubItems.Add(file.ROMSize);
                item.SubItems.Add(file.UsedSpace);
                item.SubItems.Add(file.FilePath);

                percent = (int)(++counter / (float)files.Count * 100);
                bwUpdateFileList.ReportProgress(percent, item);
                //System.Threading.Thread.Sleep(1000);
            }
        }

        private void bwUpdateFileList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            buttonsEnabled(true);
            btnBaseFolder.Text = "Select Game Folder";

            if (LV_Files.Items.Count > 0) {
                LV_Files.Items[0].Selected = true;
                contextMenuStrip1.Enabled = true;
            }

            if (e.Error != null) {
                L_Status.Text = "Status: Error updating list!";
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else {
                L_Status.Text = "Status: Game list refreshed!";
            }

            BT_BatchRename.Text = "[BETA] Batch Rename XCI";
            BT_BatchTrim.Text = "[BETA] Batch Trim XCI";
        }

        private void bwUpdateFileList_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            ListViewItem item = new ListViewItem();

            if ((ListViewItem)e.UserState == null) {
                btnBaseFolder.Text = e.ProgressPercentage.ToString() + "% Processed";
            }
            else {
                item = (ListViewItem)e.UserState;

                if (item.SubItems[chROMSize.Index].Text != item.SubItems[chUsedSpace.Index].Text) {
                    item.UseItemStyleForSubItems = false;
                    item.SubItems[chROMSize.Index].BackColor = Color.LightPink;
                }

                LV_Files.Items.Add(item);
                btnBaseFolder.Text = e.ProgressPercentage.ToString() + "% Added";
            }
        }

        private void removeOldCacheEntries() {
            // This gets rid of old entries (not sure how expensive it is to run after every UpdateFileList)
            XmlDocument cacheDoc = new XmlDocument();
            cacheDoc.Load("cache.dat");
            XmlNodeList checkNodes = cacheDoc.SelectNodes("xciorganizer/pathpair/path");
            XmlNodeList removeNodes = cacheDoc.SelectNodes("xciorganizer/pathpair");
            for (int i = checkNodes.Count - 1; i >= 0; i--) {
                bool hasOne = false;
                foreach (FileData file in files) {
                    if (checkNodes[i].InnerText.Equals(file.FilePath)) {
                        hasOne = true;
                        break;
                    }
                }
                if (!hasOne) {
                    Debug.Write(checkNodes[i].InnerText + " removed from cache\n");
                    removeNodes[i].ParentNode.RemoveChild(removeNodes[i]);
                }
            }
            cacheDoc.Save("cache.dat");
        }

        private void B_UpdateCache_Click(object sender, EventArgs e) {
            L_Status.Text = "Status: Removing old cache entries...";
            removeOldCacheEntries();
            L_Status.Text = "Status: Cache updated!";
        }

        private void LV_Files_ColumnClick(object sender, ColumnClickEventArgs e) {
            // Temporary way to sort
            bool sortChanged = false;

            if (e.Column == chReleaseID.Index) {
                sortByThis = chReleaseID.Index;
                sortChanged = true;
            }
            else if (e.Column == chGameName.Index) {
                sortByThis = chGameName.Index;
                sortChanged = true;
            }

            if(sortChanged) {
                ini.IniWriteValue("Config", "DefaultSort", sortByThis.ToString());
                UpdateFileList();
            }
        }
    }
}