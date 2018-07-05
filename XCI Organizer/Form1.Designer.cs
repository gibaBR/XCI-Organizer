namespace XCI_Organizer
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnBaseFolder = new System.Windows.Forms.Button();
            this.txbBaseFolder = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trimXCIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToSDCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRenameFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TABC_Main = new System.Windows.Forms.TabControl();
            this.TABP_XCI = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.B_ViewCert = new System.Windows.Forms.Button();
            this.B_ClearCert = new System.Windows.Forms.Button();
            this.B_ImportCert = new System.Windows.Forms.Button();
            this.B_ExportCert = new System.Windows.Forms.Button();
            this.B_CopyXCI = new System.Windows.Forms.Button();
            this.B_TrimXCI = new System.Windows.Forms.Button();
            this.TB_ExactUsedSpace = new System.Windows.Forms.TextBox();
            this.TB_ROMExactSize = new System.Windows.Forms.TextBox();
            this.TB_UsedSpace = new System.Windows.Forms.TextBox();
            this.TB_ROMSize = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TB_MKeyRev = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TB_ProdCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TB_GameRev = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TB_SDKVer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_Capacity = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PB_GameIcon = new System.Windows.Forms.PictureBox();
            this.TB_Dev = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_TID = new System.Windows.Forms.TextBox();
            this.TB_Name = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.CB_RegionName = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LB_HashedRegionSize = new System.Windows.Forms.Label();
            this.LB_ActualHash = new System.Windows.Forms.Label();
            this.LB_ExpectedHash = new System.Windows.Forms.Label();
            this.B_Extract = new System.Windows.Forms.Button();
            this.LB_DataSize = new System.Windows.Forms.Label();
            this.LB_DataOffset = new System.Windows.Forms.Label();
            this.LB_SelectedData = new System.Windows.Forms.Label();
            this.TV_Partitions = new System.Windows.Forms.TreeView();
            this.TABP_TOOLS = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BT_BatchTrim = new System.Windows.Forms.Button();
            this.B_UpdateNSWDB = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.R_BatchRenameScene = new System.Windows.Forms.RadioButton();
            this.R_BatchRenameDetailed = new System.Windows.Forms.RadioButton();
            this.BT_BatchRename = new System.Windows.Forms.Button();
            this.R_BatchRenameSimple = new System.Windows.Forms.RadioButton();
            this.BT_Refresh = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.LV_Files = new System.Windows.Forms.ListView();
            this.chTitleID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chGameName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chROMSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUsedSpace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.TABC_Main.SuspendLayout();
            this.TABP_XCI.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_GameIcon)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.TABP_TOOLS.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBaseFolder
            // 
            this.btnBaseFolder.Location = new System.Drawing.Point(2, 13);
            this.btnBaseFolder.Name = "btnBaseFolder";
            this.btnBaseFolder.Size = new System.Drawing.Size(113, 23);
            this.btnBaseFolder.TabIndex = 0;
            this.btnBaseFolder.Text = "Select Game Folder";
            this.btnBaseFolder.UseVisualStyleBackColor = true;
            this.btnBaseFolder.Click += new System.EventHandler(this.btnBaseFolder_Click);
            // 
            // txbBaseFolder
            // 
            this.txbBaseFolder.Location = new System.Drawing.Point(121, 15);
            this.txbBaseFolder.Name = "txbBaseFolder";
            this.txbBaseFolder.ReadOnly = true;
            this.txbBaseFolder.Size = new System.Drawing.Size(460, 20);
            this.txbBaseFolder.TabIndex = 1;
            this.txbBaseFolder.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showInExplorerToolStripMenuItem,
            this.trimXCIToolStripMenuItem,
            this.sendToSDCardToolStripMenuItem,
            this.autoRenameFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(168, 92);
            // 
            // showInExplorerToolStripMenuItem
            // 
            this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
            this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.showInExplorerToolStripMenuItem.Text = "Show in Explorer";
            this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
            // 
            // trimXCIToolStripMenuItem
            // 
            this.trimXCIToolStripMenuItem.Name = "trimXCIToolStripMenuItem";
            this.trimXCIToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.trimXCIToolStripMenuItem.Text = "Trim XCI";
            this.trimXCIToolStripMenuItem.Click += new System.EventHandler(this.trimXCIToolStripMenuItem_Click);
            // 
            // sendToSDCardToolStripMenuItem
            // 
            this.sendToSDCardToolStripMenuItem.Name = "sendToSDCardToolStripMenuItem";
            this.sendToSDCardToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.sendToSDCardToolStripMenuItem.Text = "Send to SD Card";
            this.sendToSDCardToolStripMenuItem.Click += new System.EventHandler(this.sendToSDCardToolStripMenuItem_Click);
            // 
            // autoRenameFileToolStripMenuItem
            // 
            this.autoRenameFileToolStripMenuItem.Name = "autoRenameFileToolStripMenuItem";
            this.autoRenameFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.autoRenameFileToolStripMenuItem.Text = "Auto Rename File";
            this.autoRenameFileToolStripMenuItem.Click += new System.EventHandler(this.autoRenameFileToolStripMenuItem_Click);
            // 
            // TABC_Main
            // 
            this.TABC_Main.Controls.Add(this.TABP_XCI);
            this.TABC_Main.Controls.Add(this.tabPage2);
            this.TABC_Main.Controls.Add(this.TABP_TOOLS);
            this.TABC_Main.Location = new System.Drawing.Point(663, 13);
            this.TABC_Main.Name = "TABC_Main";
            this.TABC_Main.SelectedIndex = 0;
            this.TABC_Main.Size = new System.Drawing.Size(582, 659);
            this.TABC_Main.TabIndex = 3;
            // 
            // TABP_XCI
            // 
            this.TABP_XCI.Controls.Add(this.groupBox1);
            this.TABP_XCI.Controls.Add(this.B_CopyXCI);
            this.TABP_XCI.Controls.Add(this.B_TrimXCI);
            this.TABP_XCI.Controls.Add(this.TB_ExactUsedSpace);
            this.TABP_XCI.Controls.Add(this.TB_ROMExactSize);
            this.TABP_XCI.Controls.Add(this.TB_UsedSpace);
            this.TABP_XCI.Controls.Add(this.TB_ROMSize);
            this.TABP_XCI.Controls.Add(this.label6);
            this.TABP_XCI.Controls.Add(this.label5);
            this.TABP_XCI.Controls.Add(this.TB_MKeyRev);
            this.TABP_XCI.Controls.Add(this.label4);
            this.TABP_XCI.Controls.Add(this.TB_ProdCode);
            this.TABP_XCI.Controls.Add(this.label8);
            this.TABP_XCI.Controls.Add(this.TB_GameRev);
            this.TABP_XCI.Controls.Add(this.label7);
            this.TABP_XCI.Controls.Add(this.TB_SDKVer);
            this.TABP_XCI.Controls.Add(this.label3);
            this.TABP_XCI.Controls.Add(this.TB_Capacity);
            this.TABP_XCI.Controls.Add(this.label2);
            this.TABP_XCI.Controls.Add(this.PB_GameIcon);
            this.TABP_XCI.Controls.Add(this.TB_Dev);
            this.TABP_XCI.Controls.Add(this.label10);
            this.TABP_XCI.Controls.Add(this.label1);
            this.TABP_XCI.Controls.Add(this.TB_TID);
            this.TABP_XCI.Controls.Add(this.TB_Name);
            this.TABP_XCI.Controls.Add(this.label9);
            this.TABP_XCI.Controls.Add(this.label11);
            this.TABP_XCI.Controls.Add(this.CB_RegionName);
            this.TABP_XCI.Location = new System.Drawing.Point(4, 22);
            this.TABP_XCI.Name = "TABP_XCI";
            this.TABP_XCI.Padding = new System.Windows.Forms.Padding(3);
            this.TABP_XCI.Size = new System.Drawing.Size(574, 633);
            this.TABP_XCI.TabIndex = 0;
            this.TABP_XCI.Text = "Game Info";
            this.TABP_XCI.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.B_ViewCert);
            this.groupBox1.Controls.Add(this.B_ClearCert);
            this.groupBox1.Controls.Add(this.B_ImportCert);
            this.groupBox1.Controls.Add(this.B_ExportCert);
            this.groupBox1.Location = new System.Drawing.Point(137, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 52);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cert";
            // 
            // B_ViewCert
            // 
            this.B_ViewCert.Location = new System.Drawing.Point(156, 19);
            this.B_ViewCert.Name = "B_ViewCert";
            this.B_ViewCert.Size = new System.Drawing.Size(66, 23);
            this.B_ViewCert.TabIndex = 3;
            this.B_ViewCert.Text = "View Cert";
            this.B_ViewCert.UseVisualStyleBackColor = true;
            this.B_ViewCert.Click += new System.EventHandler(this.B_ViewCert_Click);
            // 
            // B_ClearCert
            // 
            this.B_ClearCert.Location = new System.Drawing.Point(229, 19);
            this.B_ClearCert.Name = "B_ClearCert";
            this.B_ClearCert.Size = new System.Drawing.Size(66, 23);
            this.B_ClearCert.TabIndex = 2;
            this.B_ClearCert.Text = "Clear Cert";
            this.B_ClearCert.UseVisualStyleBackColor = true;
            this.B_ClearCert.Click += new System.EventHandler(this.B_ClearCert_Click);
            // 
            // B_ImportCert
            // 
            this.B_ImportCert.Location = new System.Drawing.Point(83, 19);
            this.B_ImportCert.Name = "B_ImportCert";
            this.B_ImportCert.Size = new System.Drawing.Size(67, 23);
            this.B_ImportCert.TabIndex = 1;
            this.B_ImportCert.Text = "Import Cert";
            this.B_ImportCert.UseVisualStyleBackColor = true;
            this.B_ImportCert.Click += new System.EventHandler(this.B_ImportCert_Click);
            // 
            // B_ExportCert
            // 
            this.B_ExportCert.Location = new System.Drawing.Point(7, 19);
            this.B_ExportCert.Name = "B_ExportCert";
            this.B_ExportCert.Size = new System.Drawing.Size(70, 23);
            this.B_ExportCert.TabIndex = 0;
            this.B_ExportCert.Text = "Export Cert";
            this.B_ExportCert.UseVisualStyleBackColor = true;
            this.B_ExportCert.Click += new System.EventHandler(this.B_ExportCert_Click);
            // 
            // B_CopyXCI
            // 
            this.B_CopyXCI.Location = new System.Drawing.Point(427, 126);
            this.B_CopyXCI.Name = "B_CopyXCI";
            this.B_CopyXCI.Size = new System.Drawing.Size(141, 23);
            this.B_CopyXCI.TabIndex = 54;
            this.B_CopyXCI.Text = "Send to SD Card";
            this.B_CopyXCI.UseVisualStyleBackColor = true;
            this.B_CopyXCI.Click += new System.EventHandler(this.B_CopyXCI_Click);
            // 
            // B_TrimXCI
            // 
            this.B_TrimXCI.Location = new System.Drawing.Point(426, 101);
            this.B_TrimXCI.Name = "B_TrimXCI";
            this.B_TrimXCI.Size = new System.Drawing.Size(142, 23);
            this.B_TrimXCI.TabIndex = 53;
            this.B_TrimXCI.Text = "Trim XCI";
            this.B_TrimXCI.UseVisualStyleBackColor = true;
            this.B_TrimXCI.Click += new System.EventHandler(this.B_TrimXCI_Click);
            // 
            // TB_ExactUsedSpace
            // 
            this.TB_ExactUsedSpace.Location = new System.Drawing.Point(233, 128);
            this.TB_ExactUsedSpace.Name = "TB_ExactUsedSpace";
            this.TB_ExactUsedSpace.ReadOnly = true;
            this.TB_ExactUsedSpace.Size = new System.Drawing.Size(188, 20);
            this.TB_ExactUsedSpace.TabIndex = 52;
            // 
            // TB_ROMExactSize
            // 
            this.TB_ROMExactSize.Location = new System.Drawing.Point(233, 101);
            this.TB_ROMExactSize.Name = "TB_ROMExactSize";
            this.TB_ROMExactSize.ReadOnly = true;
            this.TB_ROMExactSize.Size = new System.Drawing.Size(188, 20);
            this.TB_ROMExactSize.TabIndex = 51;
            // 
            // TB_UsedSpace
            // 
            this.TB_UsedSpace.Location = new System.Drawing.Point(158, 128);
            this.TB_UsedSpace.Name = "TB_UsedSpace";
            this.TB_UsedSpace.ReadOnly = true;
            this.TB_UsedSpace.Size = new System.Drawing.Size(69, 20);
            this.TB_UsedSpace.TabIndex = 50;
            // 
            // TB_ROMSize
            // 
            this.TB_ROMSize.Location = new System.Drawing.Point(158, 101);
            this.TB_ROMSize.Name = "TB_ROMSize";
            this.TB_ROMSize.ReadOnly = true;
            this.TB_ROMSize.Size = new System.Drawing.Size(69, 20);
            this.TB_ROMSize.TabIndex = 49;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 48;
            this.label6.Text = "Used Space:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "ROM Size:";
            // 
            // TB_MKeyRev
            // 
            this.TB_MKeyRev.Location = new System.Drawing.Point(427, 69);
            this.TB_MKeyRev.Name = "TB_MKeyRev";
            this.TB_MKeyRev.ReadOnly = true;
            this.TB_MKeyRev.Size = new System.Drawing.Size(141, 20);
            this.TB_MKeyRev.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(424, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "MasterKey Revision:";
            // 
            // TB_ProdCode
            // 
            this.TB_ProdCode.Location = new System.Drawing.Point(257, 69);
            this.TB_ProdCode.Name = "TB_ProdCode";
            this.TB_ProdCode.ReadOnly = true;
            this.TB_ProdCode.Size = new System.Drawing.Size(90, 20);
            this.TB_ProdCode.TabIndex = 44;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(254, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 43;
            this.label8.Text = "Product Code:";
            // 
            // TB_GameRev
            // 
            this.TB_GameRev.Location = new System.Drawing.Point(157, 69);
            this.TB_GameRev.Name = "TB_GameRev";
            this.TB_GameRev.ReadOnly = true;
            this.TB_GameRev.Size = new System.Drawing.Size(95, 20);
            this.TB_GameRev.TabIndex = 42;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(154, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Game Revision:";
            // 
            // TB_SDKVer
            // 
            this.TB_SDKVer.Location = new System.Drawing.Point(352, 69);
            this.TB_SDKVer.Name = "TB_SDKVer";
            this.TB_SDKVer.ReadOnly = true;
            this.TB_SDKVer.Size = new System.Drawing.Size(69, 20);
            this.TB_SDKVer.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(349, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "SDK Version:";
            // 
            // TB_Capacity
            // 
            this.TB_Capacity.Location = new System.Drawing.Point(10, 117);
            this.TB_Capacity.Name = "TB_Capacity";
            this.TB_Capacity.ReadOnly = true;
            this.TB_Capacity.Size = new System.Drawing.Size(69, 20);
            this.TB_Capacity.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Cart Size:";
            // 
            // PB_GameIcon
            // 
            this.PB_GameIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PB_GameIcon.Location = new System.Drawing.Point(89, 221);
            this.PB_GameIcon.Name = "PB_GameIcon";
            this.PB_GameIcon.Size = new System.Drawing.Size(399, 399);
            this.PB_GameIcon.TabIndex = 26;
            this.PB_GameIcon.TabStop = false;
            this.PB_GameIcon.Click += new System.EventHandler(this.PB_GameIcon_Click);
            // 
            // TB_Dev
            // 
            this.TB_Dev.Location = new System.Drawing.Point(426, 23);
            this.TB_Dev.Name = "TB_Dev";
            this.TB_Dev.ReadOnly = true;
            this.TB_Dev.Size = new System.Drawing.Size(142, 20);
            this.TB_Dev.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(423, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Developer:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Title ID:";
            // 
            // TB_TID
            // 
            this.TB_TID.Location = new System.Drawing.Point(10, 69);
            this.TB_TID.Name = "TB_TID";
            this.TB_TID.ReadOnly = true;
            this.TB_TID.Size = new System.Drawing.Size(142, 20);
            this.TB_TID.TabIndex = 24;
            // 
            // TB_Name
            // 
            this.TB_Name.Location = new System.Drawing.Point(158, 23);
            this.TB_Name.Name = "TB_Name";
            this.TB_Name.ReadOnly = true;
            this.TB_Name.Size = new System.Drawing.Size(263, 20);
            this.TB_Name.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(155, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Name:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Language:";
            // 
            // CB_RegionName
            // 
            this.CB_RegionName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_RegionName.FormattingEnabled = true;
            this.CB_RegionName.Location = new System.Drawing.Point(9, 23);
            this.CB_RegionName.Name = "CB_RegionName";
            this.CB_RegionName.Size = new System.Drawing.Size(143, 21);
            this.CB_RegionName.TabIndex = 17;
            this.CB_RegionName.SelectedIndexChanged += new System.EventHandler(this.CB_RegionName_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.LB_HashedRegionSize);
            this.tabPage2.Controls.Add(this.LB_ActualHash);
            this.tabPage2.Controls.Add(this.LB_ExpectedHash);
            this.tabPage2.Controls.Add(this.B_Extract);
            this.tabPage2.Controls.Add(this.LB_DataSize);
            this.tabPage2.Controls.Add(this.LB_DataOffset);
            this.tabPage2.Controls.Add(this.LB_SelectedData);
            this.tabPage2.Controls.Add(this.TV_Partitions);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(574, 633);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Partitions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LB_HashedRegionSize
            // 
            this.LB_HashedRegionSize.AutoSize = true;
            this.LB_HashedRegionSize.Location = new System.Drawing.Point(6, 416);
            this.LB_HashedRegionSize.Name = "LB_HashedRegionSize";
            this.LB_HashedRegionSize.Size = new System.Drawing.Size(101, 13);
            this.LB_HashedRegionSize.TabIndex = 7;
            this.LB_HashedRegionSize.Text = "HashedRegionSize:";
            // 
            // LB_ActualHash
            // 
            this.LB_ActualHash.AutoSize = true;
            this.LB_ActualHash.Location = new System.Drawing.Point(6, 443);
            this.LB_ActualHash.Name = "LB_ActualHash";
            this.LB_ActualHash.Size = new System.Drawing.Size(68, 13);
            this.LB_ActualHash.TabIndex = 6;
            this.LB_ActualHash.Text = "Actual Hash:";
            // 
            // LB_ExpectedHash
            // 
            this.LB_ExpectedHash.AutoSize = true;
            this.LB_ExpectedHash.Location = new System.Drawing.Point(6, 430);
            this.LB_ExpectedHash.Name = "LB_ExpectedHash";
            this.LB_ExpectedHash.Size = new System.Drawing.Size(73, 13);
            this.LB_ExpectedHash.TabIndex = 5;
            this.LB_ExpectedHash.Text = "Header Hash:";
            // 
            // B_Extract
            // 
            this.B_Extract.Enabled = false;
            this.B_Extract.Location = new System.Drawing.Point(523, 367);
            this.B_Extract.Name = "B_Extract";
            this.B_Extract.Size = new System.Drawing.Size(48, 23);
            this.B_Extract.TabIndex = 4;
            this.B_Extract.Text = "Extract";
            this.B_Extract.UseVisualStyleBackColor = true;
            this.B_Extract.Click += new System.EventHandler(this.B_Extract_Click);
            // 
            // LB_DataSize
            // 
            this.LB_DataSize.AutoSize = true;
            this.LB_DataSize.Location = new System.Drawing.Point(6, 403);
            this.LB_DataSize.Name = "LB_DataSize";
            this.LB_DataSize.Size = new System.Drawing.Size(30, 13);
            this.LB_DataSize.TabIndex = 3;
            this.LB_DataSize.Text = "Size:";
            // 
            // LB_DataOffset
            // 
            this.LB_DataOffset.AutoSize = true;
            this.LB_DataOffset.Location = new System.Drawing.Point(6, 390);
            this.LB_DataOffset.Name = "LB_DataOffset";
            this.LB_DataOffset.Size = new System.Drawing.Size(38, 13);
            this.LB_DataOffset.TabIndex = 2;
            this.LB_DataOffset.Text = "Offset:";
            // 
            // LB_SelectedData
            // 
            this.LB_SelectedData.AutoSize = true;
            this.LB_SelectedData.Location = new System.Drawing.Point(6, 367);
            this.LB_SelectedData.Name = "LB_SelectedData";
            this.LB_SelectedData.Size = new System.Drawing.Size(51, 13);
            this.LB_SelectedData.TabIndex = 1;
            this.LB_SelectedData.Text = "FileName";
            // 
            // TV_Partitions
            // 
            this.TV_Partitions.Dock = System.Windows.Forms.DockStyle.Top;
            this.TV_Partitions.HideSelection = false;
            this.TV_Partitions.Location = new System.Drawing.Point(3, 3);
            this.TV_Partitions.Name = "TV_Partitions";
            this.TV_Partitions.Size = new System.Drawing.Size(568, 361);
            this.TV_Partitions.TabIndex = 0;
            this.TV_Partitions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TV_Partitions_AfterSelect);
            // 
            // TABP_TOOLS
            // 
            this.TABP_TOOLS.Controls.Add(this.groupBox3);
            this.TABP_TOOLS.Controls.Add(this.groupBox2);
            this.TABP_TOOLS.Location = new System.Drawing.Point(4, 22);
            this.TABP_TOOLS.Name = "TABP_TOOLS";
            this.TABP_TOOLS.Padding = new System.Windows.Forms.Padding(3);
            this.TABP_TOOLS.Size = new System.Drawing.Size(574, 633);
            this.TABP_TOOLS.TabIndex = 2;
            this.TABP_TOOLS.Text = "Tools";
            this.TABP_TOOLS.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BT_BatchTrim);
            this.groupBox3.Controls.Add(this.B_UpdateNSWDB);
            this.groupBox3.Location = new System.Drawing.Point(141, 202);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(300, 151);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Miscellaneous";
            // 
            // BT_BatchTrim
            // 
            this.BT_BatchTrim.Location = new System.Drawing.Point(71, 38);
            this.BT_BatchTrim.Name = "BT_BatchTrim";
            this.BT_BatchTrim.Size = new System.Drawing.Size(153, 23);
            this.BT_BatchTrim.TabIndex = 1;
            this.BT_BatchTrim.Text = "[BETA] Batch Trim XCI";
            this.BT_BatchTrim.UseVisualStyleBackColor = true;
            this.BT_BatchTrim.Click += new System.EventHandler(this.BT_BatchTrim_Click);
            // 
            // B_UpdateNSWDB
            // 
            this.B_UpdateNSWDB.Location = new System.Drawing.Point(71, 93);
            this.B_UpdateNSWDB.Name = "B_UpdateNSWDB";
            this.B_UpdateNSWDB.Size = new System.Drawing.Size(153, 23);
            this.B_UpdateNSWDB.TabIndex = 2;
            this.B_UpdateNSWDB.Text = "Update NSWDB";
            this.B_UpdateNSWDB.UseVisualStyleBackColor = true;
            this.B_UpdateNSWDB.Click += new System.EventHandler(this.B_UpdateNSWDB_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.R_BatchRenameScene);
            this.groupBox2.Controls.Add(this.R_BatchRenameDetailed);
            this.groupBox2.Controls.Add(this.BT_BatchRename);
            this.groupBox2.Controls.Add(this.R_BatchRenameSimple);
            this.groupBox2.Location = new System.Drawing.Point(141, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 165);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rename Options";
            // 
            // R_BatchRenameScene
            // 
            this.R_BatchRenameScene.AutoSize = true;
            this.R_BatchRenameScene.Location = new System.Drawing.Point(18, 123);
            this.R_BatchRenameScene.Name = "R_BatchRenameScene";
            this.R_BatchRenameScene.Size = new System.Drawing.Size(163, 17);
            this.R_BatchRenameScene.TabIndex = 5;
            this.R_BatchRenameScene.Text = "Scene          \"SCENENAME\"";
            this.R_BatchRenameScene.UseVisualStyleBackColor = true;
            // 
            // R_BatchRenameDetailed
            // 
            this.R_BatchRenameDetailed.AutoSize = true;
            this.R_BatchRenameDetailed.Location = new System.Drawing.Point(18, 100);
            this.R_BatchRenameDetailed.Name = "R_BatchRenameDetailed";
            this.R_BatchRenameDetailed.Size = new System.Drawing.Size(272, 17);
            this.R_BatchRenameDetailed.TabIndex = 4;
            this.R_BatchRenameDetailed.Text = "Detailed       \"ID - NAME (REGION) (LANGUAGES)\"";
            this.R_BatchRenameDetailed.UseVisualStyleBackColor = true;
            // 
            // BT_BatchRename
            // 
            this.BT_BatchRename.Location = new System.Drawing.Point(71, 34);
            this.BT_BatchRename.Name = "BT_BatchRename";
            this.BT_BatchRename.Size = new System.Drawing.Size(153, 23);
            this.BT_BatchRename.TabIndex = 0;
            this.BT_BatchRename.Text = "[BETA] Batch Rename XCI";
            this.BT_BatchRename.UseVisualStyleBackColor = true;
            this.BT_BatchRename.Click += new System.EventHandler(this.BT_BatchRename_Click);
            // 
            // R_BatchRenameSimple
            // 
            this.R_BatchRenameSimple.AutoSize = true;
            this.R_BatchRenameSimple.Checked = true;
            this.R_BatchRenameSimple.Location = new System.Drawing.Point(18, 77);
            this.R_BatchRenameSimple.Name = "R_BatchRenameSimple";
            this.R_BatchRenameSimple.Size = new System.Drawing.Size(175, 17);
            this.R_BatchRenameSimple.TabIndex = 3;
            this.R_BatchRenameSimple.TabStop = true;
            this.R_BatchRenameSimple.Text = "Simple         \"NAME (REGION)\"";
            this.R_BatchRenameSimple.UseVisualStyleBackColor = true;
            // 
            // BT_Refresh
            // 
            this.BT_Refresh.Location = new System.Drawing.Point(587, 13);
            this.BT_Refresh.Name = "BT_Refresh";
            this.BT_Refresh.Size = new System.Drawing.Size(70, 23);
            this.BT_Refresh.TabIndex = 5;
            this.BT_Refresh.Text = "Refresh";
            this.BT_Refresh.UseVisualStyleBackColor = true;
            this.BT_Refresh.Click += new System.EventHandler(this.BT_Refresh_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // LV_Files
            // 
            this.LV_Files.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTitleID,
            this.chGameName,
            this.chROMSize,
            this.chUsedSpace,
            this.chFileName});
            this.LV_Files.ContextMenuStrip = this.contextMenuStrip1;
            this.LV_Files.FullRowSelect = true;
            this.LV_Files.GridLines = true;
            this.LV_Files.Location = new System.Drawing.Point(2, 42);
            this.LV_Files.MultiSelect = false;
            this.LV_Files.Name = "LV_Files";
            this.LV_Files.Size = new System.Drawing.Size(655, 626);
            this.LV_Files.TabIndex = 6;
            this.LV_Files.UseCompatibleStateImageBehavior = false;
            this.LV_Files.View = System.Windows.Forms.View.Details;
            this.LV_Files.SelectedIndexChanged += new System.EventHandler(this.LV_Files_SelectedIndexChanged);
            // 
            // chTitleID
            // 
            this.chTitleID.Text = "Title ID";
            this.chTitleID.Width = 104;
            // 
            // chGameName
            // 
            this.chGameName.Text = "Game Name";
            this.chGameName.Width = 350;
            // 
            // chROMSize
            // 
            this.chROMSize.Text = "ROM size";
            this.chROMSize.Width = 87;
            // 
            // chUsedSpace
            // 
            this.chUsedSpace.Text = "Used space";
            this.chUsedSpace.Width = 87;
            // 
            // chFileName
            // 
            this.chFileName.Text = "File name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 676);
            this.Controls.Add(this.LV_Files);
            this.Controls.Add(this.BT_Refresh);
            this.Controls.Add(this.TABC_Main);
            this.Controls.Add(this.txbBaseFolder);
            this.Controls.Add(this.btnBaseFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XCI Organizer";
            this.contextMenuStrip1.ResumeLayout(false);
            this.TABC_Main.ResumeLayout(false);
            this.TABP_XCI.ResumeLayout(false);
            this.TABP_XCI.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_GameIcon)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.TABP_TOOLS.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBaseFolder;
        private System.Windows.Forms.TextBox txbBaseFolder;
        private System.Windows.Forms.TabControl TABC_Main;
        private System.Windows.Forms.TabPage TABP_XCI;
        private System.Windows.Forms.TextBox TB_Dev;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TB_Name;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox CB_RegionName;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label LB_HashedRegionSize;
        private System.Windows.Forms.Label LB_ActualHash;
        private System.Windows.Forms.Label LB_ExpectedHash;
        private System.Windows.Forms.Button B_Extract;
        private System.Windows.Forms.Label LB_DataSize;
        private System.Windows.Forms.Label LB_DataOffset;
        private System.Windows.Forms.Label LB_SelectedData;
        private System.Windows.Forms.TreeView TV_Partitions;
        private System.Windows.Forms.PictureBox PB_GameIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_TID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TB_SDKVer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_Capacity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button B_TrimXCI;
        private System.Windows.Forms.TextBox TB_ExactUsedSpace;
        private System.Windows.Forms.TextBox TB_ROMExactSize;
        private System.Windows.Forms.TextBox TB_UsedSpace;
        private System.Windows.Forms.TextBox TB_ROMSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TB_MKeyRev;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_ProdCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TB_GameRev;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button B_CopyXCI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button B_ViewCert;
        private System.Windows.Forms.Button B_ClearCert;
        private System.Windows.Forms.Button B_ImportCert;
        private System.Windows.Forms.Button B_ExportCert;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trimXCIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToSDCardToolStripMenuItem;
        private System.Windows.Forms.Button BT_Refresh;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabPage TABP_TOOLS;
        private System.Windows.Forms.Button BT_BatchRename;
        private System.Windows.Forms.Button BT_BatchTrim;
        private System.Windows.Forms.ToolStripMenuItem autoRenameFileToolStripMenuItem;
        private System.Windows.Forms.Button B_UpdateNSWDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton R_BatchRenameScene;
        private System.Windows.Forms.RadioButton R_BatchRenameDetailed;
        private System.Windows.Forms.RadioButton R_BatchRenameSimple;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView LV_Files;
        private System.Windows.Forms.ColumnHeader chTitleID;
        private System.Windows.Forms.ColumnHeader chGameName;
        private System.Windows.Forms.ColumnHeader chROMSize;
        private System.Windows.Forms.ColumnHeader chUsedSpace;
        private System.Windows.Forms.ColumnHeader chFileName;
    }
}

