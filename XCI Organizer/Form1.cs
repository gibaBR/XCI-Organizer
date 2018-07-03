using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCI_Explorer;
using XCI_Organizer.Helpers;
using XCI_Organizer.XTSSharp;

namespace XCI_Organizer
{
    public partial class Form1 : Form
    {

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

        public byte[] NcaHeaderEncryptionKey1_Prod;
        public byte[] NcaHeaderEncryptionKey2_Prod;
        public string Mkey;
        public string selectedFile;
        public double UsedSize;

        public IniFile ini;
        
            private Image[] Icons = new Image[16];
        private TreeViewFileSystem TV_Parti;
        private BetterTreeNode rootNode;
        public List<char> chars = new List<char>();

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



        public Form1() {
            InitializeComponent();
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = "XCI Organizer v0.0.1";// + assemblyVersion;

            if (!File.Exists("keys.txt")) {
                if (File.Exists("Get-keys.txt.bat") && MessageBox.Show("keys.txt is missing.\nDo you want to automatically download it now?", "XCI Organizer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "Get-keys.txt.bat"
                    };
                    process.Start();
                    process.WaitForExit();
                }

                if (!File.Exists("keys.txt"))
                {
                    MessageBox.Show("keys.txt failed to load.\nPlease include keys.txt in this location.");
                    Environment.Exit(0);
                }
            }
            if (!File.Exists("hactool.exe"))
            {
                MessageBox.Show("hactool.exe is missing.");
                Environment.Exit(0);
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
            } catch {
                return false;
            }
        }

        private void btnBaseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderFileDialog = new FolderBrowserDialog();
            folderFileDialog.Description = "Select the base folder for your collection";
            if (folderFileDialog.ShowDialog() == DialogResult.OK) {
                ini.IniWriteValue("Config", "BaseFolder", folderFileDialog.SelectedPath);
                UpdateFileList();
            }
        }

        private void UpdateFileList () {
            string selectedPath = ini.IniReadValue("Config", "BaseFolder");
            contextMenuStrip1.Enabled = false;

            if (selectedPath.Trim() != "") {
                txbBaseFolder.Text = selectedPath;
                lboxFiles.Items.Clear();
                ClearFields();
                string[] directories = Directory.GetDirectories(selectedPath);

                List<string> files = Util.GetXCIsInFolder(selectedPath);

                foreach (string file in files) {
                    lboxFiles.Items.Add(file);
                }

                if (lboxFiles.Items.Count > 0) {
                    lboxFiles.SelectedIndex = 0;
                    contextMenuStrip1.Enabled = true;
                }
            }
        }

        private void lboxFiles_SelectedIndexChanged(object sender, EventArgs e) {
            ClearFields();
            selectedFile = lboxFiles.SelectedItem.ToString();
            if (selectedFile.Trim() != "") {
                ProcessFile();
            }
        }

        private void ProcessFile() {
            if (Util.CheckXCI(selectedFile)) {
                LoadXCI();
            } else {
                //TB_File.Text = null;
                MessageBox.Show("Unsupported file.");
            }
        }

        private void TrimXCI() {
            if (Util.checkFile(selectedFile)) {
                if (MessageBox.Show("Trim XCI?", "XCI Organizer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    if (!TB_ROMExactSize.Text.Equals(TB_ExactUsedSpace.Text)) {
                        FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Write);
                        fileStream.SetLength((long)UsedSize);
                        fileStream.Close();
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
                        while (num >= 1024.0 && num2 < array.Length - 1)
                        {
                            num2++;
                            num /= 1024.0;
                        }
                        TB_ROMSize.Text = $"{num:0.##} {array[num2]}";
                        double num3 = UsedSize = (double)(XCI.XCI_Headers[0].CardSize2 * 512 + 512);
                        TB_ExactUsedSpace.Text = "(" + num3.ToString() + " bytes)";
                        num2 = 0;
                        while (num3 >= 1024.0 && num2 < array.Length - 1)
                        {
                            num2++;
                            num3 /= 1024.0;
                        }
                        TB_UsedSpace.Text = $"{num3:0.##} {array[num2]}";
                    }
                    else
                    {
                        MessageBox.Show("No trimming needed!");
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found");
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
                using (FileStream fileStream = File.OpenRead(selectedFile))
                {
                    for (int si = 0; si < SecureSize.Length; si++)
                    {
                        if (SecureSize[si] > 0x4E20000) continue;
                        try
                        {
                            File.Delete("meta");
                            Directory.Delete("data", true);
                        }
                        catch { }

                        using (FileStream fileStream2 = File.OpenWrite("meta"))
                        {
                            fileStream.Position = SecureOffset[si];
                            byte[] buffer = new byte[8192];
                            long num = SecureSize[si];
                            int num2;
                            while ((num2 = fileStream.Read(buffer, 0, 8192)) > 0 && num > 0)
                            {
                                fileStream2.Write(buffer, 0, num2);
                                num -= num2;
                            }
                            fileStream2.Close();
                        }

                        Process process = new Process();
                        process.StartInfo = new ProcessStartInfo
                        {
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

/*

*/
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
            NCA.NCA_Headers[0] = new NCA.NCA_Header(DecryptNCAHeader(gameNcaOffset));
            TB_TID.Text = NCA.NCA_Headers[0].TitleID.ToString("X");
            TB_SDKVer.Text = $"{NCA.NCA_Headers[0].SDKVersion4}.{NCA.NCA_Headers[0].SDKVersion3}.{NCA.NCA_Headers[0].SDKVersion2}.{NCA.NCA_Headers[0].SDKVersion1}";
            TB_MKeyRev.Text = Util.GetMkey(NCA.NCA_Headers[0].MasterKeyRev);
        }

        public byte[] DecryptNCAHeader(long offset)
        {
            byte[] array = new byte[3072];
            if (File.Exists(selectedFile))
            {
                FileStream fileStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read);
                fileStream.Position = offset;
                fileStream.Read(array, 0, 3072);
                File.WriteAllBytes(selectedFile + ".tmp", array);
                Xts xts = XtsAes128.Create(NcaHeaderEncryptionKey1_Prod, NcaHeaderEncryptionKey2_Prod);
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

        private void LoadPartitons()  {
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

        private void CB_RegionName_SelectedIndexChanged(object sender, EventArgs e) {
            int num = Array.FindIndex(Language, (string element) => element.StartsWith(CB_RegionName.Text, StringComparison.Ordinal));
            PB_GameIcon.BackgroundImage = Icons[num];
            TB_Name.Text = NACP.NACP_Strings[num].GameName;
            TB_Dev.Text = NACP.NACP_Strings[num].GameAuthor;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txbBaseFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e) {
            // opens the folder in explorer

            if ((lboxFiles.Items.Count > 0) && (lboxFiles.SelectedIndex >= 0)) {
                System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(selectedFile));
            }
        }

        private void trimXCIToolStripMenuItem_Click(object sender, EventArgs e) {
            TrimXCI();
        }

        private void B_TrimXCI_Click(object sender, EventArgs e) {
            TrimXCI();
        }

        private void BT_Refresh_Click(object sender, EventArgs e)
        {
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
            else
            {
                MessageBox.Show("File not found");
            }
        }

        private void B_CopyXCI_Click(object sender, EventArgs e)
        {
            SendFileToSD();
        }

        private void SendFileToSD()
        {
            MessageBox.Show("Soon®");
        }

        private void sendToSDCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendFileToSD();
        }

        private void B_ClearCert_Click(object sender, EventArgs e)
        {
            if (Util.checkFile(selectedFile))
            {
                if (MessageBox.Show("The cert will be deleted permanently.\nContinue?", "XCI Explorer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (Stream stream = File.Open(selectedFile, FileMode.Open))
                    {
                        byte[] array = new byte[512];
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] = byte.MaxValue;
                        }
                        stream.Position = 28672L;
                        stream.Write(array, 0, array.Length);
                        MessageBox.Show("cert deleted.");
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found");
            }

        }
    }
}
