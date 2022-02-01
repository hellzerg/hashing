namespace Hashing
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.helperMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMD5 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSHA1 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSHA256 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSHA384 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSHA512 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemCRC32 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRIPEMD160 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topPanel = new System.Windows.Forms.Panel();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnFindIdenticals = new System.Windows.Forms.Button();
            this.btnSaveJson = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblversion = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.botPanel = new System.Windows.Forms.Panel();
            this.btnCancelHashing = new System.Windows.Forms.Button();
            this.lblCalculating = new System.Windows.Forms.Label();
            this.SumView = new Hashing.MoonTree();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.helperMenu.SuspendLayout();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.botPanel.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // helperMenu
            // 
            this.helperMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.helperMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.helperMenu.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.helperMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.helperMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripMenuItem4,
            this.removeToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripSeparator2,
            this.toolStripMenuItem5,
            this.toolStripSeparator3,
            this.clearToolStripMenuItem});
            this.helperMenu.Name = "helperMenu";
            this.helperMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.helperMenu.Size = new System.Drawing.Size(252, 230);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem4.Image")));
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(251, 26);
            this.toolStripMenuItem4.Text = "Show file";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.removeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripMenuItem.Image")));
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.removeToolStripMenuItem.Text = "Delete";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(248, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 26);
            this.toolStripMenuItem1.Text = "Check in VirusTotal...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(251, 26);
            this.toolStripMenuItem2.Text = "Search with DuckDuckGo...";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem3.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem3.Image")));
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(251, 26);
            this.toolStripMenuItem3.Text = "Search with Google...";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(248, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemMD5,
            this.itemSHA1,
            this.itemSHA256,
            this.itemSHA384,
            this.itemSHA512,
            this.itemCRC32,
            this.itemRIPEMD160});
            this.toolStripMenuItem5.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(251, 26);
            this.toolStripMenuItem5.Text = "Hashes";
            // 
            // itemMD5
            // 
            this.itemMD5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemMD5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemMD5.ForeColor = System.Drawing.Color.White;
            this.itemMD5.Name = "itemMD5";
            this.itemMD5.Size = new System.Drawing.Size(180, 24);
            this.itemMD5.Tag = "md5";
            this.itemMD5.Text = "MD5";
            // 
            // itemSHA1
            // 
            this.itemSHA1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemSHA1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemSHA1.ForeColor = System.Drawing.Color.White;
            this.itemSHA1.Name = "itemSHA1";
            this.itemSHA1.Size = new System.Drawing.Size(180, 24);
            this.itemSHA1.Tag = "sha1";
            this.itemSHA1.Text = "SHA1";
            // 
            // itemSHA256
            // 
            this.itemSHA256.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemSHA256.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemSHA256.ForeColor = System.Drawing.Color.White;
            this.itemSHA256.Name = "itemSHA256";
            this.itemSHA256.Size = new System.Drawing.Size(180, 24);
            this.itemSHA256.Tag = "sha256";
            this.itemSHA256.Text = "SHA256";
            // 
            // itemSHA384
            // 
            this.itemSHA384.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemSHA384.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemSHA384.ForeColor = System.Drawing.Color.White;
            this.itemSHA384.Name = "itemSHA384";
            this.itemSHA384.Size = new System.Drawing.Size(180, 24);
            this.itemSHA384.Tag = "sha384";
            this.itemSHA384.Text = "SHA384";
            // 
            // itemSHA512
            // 
            this.itemSHA512.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemSHA512.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemSHA512.ForeColor = System.Drawing.Color.White;
            this.itemSHA512.Name = "itemSHA512";
            this.itemSHA512.Size = new System.Drawing.Size(180, 24);
            this.itemSHA512.Tag = "sha512";
            this.itemSHA512.Text = "SHA512";
            // 
            // itemCRC32
            // 
            this.itemCRC32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemCRC32.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemCRC32.ForeColor = System.Drawing.Color.White;
            this.itemCRC32.Name = "itemCRC32";
            this.itemCRC32.Size = new System.Drawing.Size(180, 24);
            this.itemCRC32.Tag = "crc32";
            this.itemCRC32.Text = "CRC32";
            // 
            // itemRIPEMD160
            // 
            this.itemRIPEMD160.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.itemRIPEMD160.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemRIPEMD160.ForeColor = System.Drawing.Color.White;
            this.itemRIPEMD160.Name = "itemRIPEMD160";
            this.itemRIPEMD160.Size = new System.Drawing.Size(180, 24);
            this.itemRIPEMD160.Tag = "ripemd160";
            this.itemRIPEMD160.Text = "RIPEMD160";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(248, 6);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.clearToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clearToolStripMenuItem.Image")));
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.clearToolStripMenuItem.Text = "Clear all";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // topPanel
            // 
            this.topPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topPanel.Controls.Add(this.btnUpdate);
            this.topPanel.Controls.Add(this.btnCalculate);
            this.topPanel.Controls.Add(this.btnBrowse);
            this.topPanel.Controls.Add(this.txtPath);
            this.topPanel.Controls.Add(this.btnCompare);
            this.topPanel.Controls.Add(this.btnOptions);
            this.topPanel.Controls.Add(this.btnFindIdenticals);
            this.topPanel.Controls.Add(this.btnSaveJson);
            this.topPanel.Controls.Add(this.pictureBox1);
            this.topPanel.Controls.Add(this.lblversion);
            this.topPanel.Controls.Add(this.label24);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(2);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(987, 72);
            this.topPanel.TabIndex = 7;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnUpdate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnUpdate.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(824, 5);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(156, 27);
            this.btnUpdate.TabIndex = 81;
            this.btnUpdate.Tag = "themeable";
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalculate.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCalculate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCalculate.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCalculate.FlatAppearance.BorderSize = 0;
            this.btnCalculate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCalculate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.ForeColor = System.Drawing.Color.White;
            this.btnCalculate.Location = new System.Drawing.Point(824, 36);
            this.btnCalculate.Margin = new System.Windows.Forms.Padding(2);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(156, 27);
            this.btnCalculate.TabIndex = 80;
            this.btnCalculate.Tag = "themeable";
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.Location = new System.Drawing.Point(782, 36);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 27);
            this.btnBrowse.TabIndex = 79;
            this.btnBrowse.Tag = "themeable";
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPath.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.ForeColor = System.Drawing.Color.Silver;
            this.txtPath.Location = new System.Drawing.Point(184, 37);
            this.txtPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(594, 25);
            this.txtPath.TabIndex = 78;
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCompare.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCompare.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatAppearance.BorderSize = 0;
            this.btnCompare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompare.ForeColor = System.Drawing.Color.White;
            this.btnCompare.Location = new System.Drawing.Point(344, 5);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(156, 27);
            this.btnCompare.TabIndex = 77;
            this.btnCompare.Tag = "themeable";
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = false;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptions.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnOptions.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOptions.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnOptions.FlatAppearance.BorderSize = 0;
            this.btnOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnOptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptions.ForeColor = System.Drawing.Color.White;
            this.btnOptions.Location = new System.Drawing.Point(664, 5);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(2);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(156, 27);
            this.btnOptions.TabIndex = 76;
            this.btnOptions.Tag = "themeable";
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = false;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // btnFindIdenticals
            // 
            this.btnFindIdenticals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindIdenticals.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnFindIdenticals.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFindIdenticals.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnFindIdenticals.FlatAppearance.BorderSize = 0;
            this.btnFindIdenticals.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnFindIdenticals.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnFindIdenticals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindIdenticals.ForeColor = System.Drawing.Color.White;
            this.btnFindIdenticals.Location = new System.Drawing.Point(184, 5);
            this.btnFindIdenticals.Margin = new System.Windows.Forms.Padding(2);
            this.btnFindIdenticals.Name = "btnFindIdenticals";
            this.btnFindIdenticals.Size = new System.Drawing.Size(156, 27);
            this.btnFindIdenticals.TabIndex = 74;
            this.btnFindIdenticals.Tag = "themeable";
            this.btnFindIdenticals.Text = "Find Identicals";
            this.btnFindIdenticals.UseVisualStyleBackColor = false;
            this.btnFindIdenticals.Click += new System.EventHandler(this.btnFindIdenticals_Click);
            // 
            // btnSaveJson
            // 
            this.btnSaveJson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveJson.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSaveJson.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveJson.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSaveJson.FlatAppearance.BorderSize = 0;
            this.btnSaveJson.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnSaveJson.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnSaveJson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveJson.ForeColor = System.Drawing.Color.White;
            this.btnSaveJson.Location = new System.Drawing.Point(504, 5);
            this.btnSaveJson.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveJson.Name = "btnSaveJson";
            this.btnSaveJson.Size = new System.Drawing.Size(156, 27);
            this.btnSaveJson.TabIndex = 71;
            this.btnSaveJson.Tag = "themeable";
            this.btnSaveJson.Text = "Save as JSON";
            this.btnSaveJson.UseVisualStyleBackColor = false;
            this.btnSaveJson.Click += new System.EventHandler(this.btnSaveJson_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(44, 43);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblversion
            // 
            this.lblversion.AutoSize = true;
            this.lblversion.ForeColor = System.Drawing.Color.Silver;
            this.lblversion.Location = new System.Drawing.Point(72, 37);
            this.lblversion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblversion.Name = "lblversion";
            this.lblversion.Size = new System.Drawing.Size(49, 15);
            this.lblversion.TabIndex = 4;
            this.lblversion.Text = "Version:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(70, 7);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(82, 25);
            this.label24.TabIndex = 3;
            this.label24.Text = "Hashing";
            // 
            // botPanel
            // 
            this.botPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.botPanel.Controls.Add(this.btnCancelHashing);
            this.botPanel.Controls.Add(this.lblCalculating);
            this.botPanel.Controls.Add(this.SumView);
            this.botPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.botPanel.Location = new System.Drawing.Point(0, 72);
            this.botPanel.Margin = new System.Windows.Forms.Padding(2);
            this.botPanel.Name = "botPanel";
            this.botPanel.Size = new System.Drawing.Size(987, 557);
            this.botPanel.TabIndex = 8;
            this.botPanel.Resize += new System.EventHandler(this.botPanel_Resize);
            // 
            // btnCancelHashing
            // 
            this.btnCancelHashing.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCancelHashing.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelHashing.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCancelHashing.FlatAppearance.BorderSize = 0;
            this.btnCancelHashing.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancelHashing.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancelHashing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelHashing.ForeColor = System.Drawing.Color.White;
            this.btnCancelHashing.Location = new System.Drawing.Point(416, 301);
            this.btnCancelHashing.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancelHashing.Name = "btnCancelHashing";
            this.btnCancelHashing.Size = new System.Drawing.Size(153, 27);
            this.btnCancelHashing.TabIndex = 82;
            this.btnCancelHashing.Tag = "themeable";
            this.btnCancelHashing.Text = "Cancel";
            this.btnCancelHashing.UseVisualStyleBackColor = false;
            this.btnCancelHashing.Visible = false;
            this.btnCancelHashing.Click += new System.EventHandler(this.btnCancelHashing_Click);
            // 
            // lblCalculating
            // 
            this.lblCalculating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCalculating.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalculating.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCalculating.Location = new System.Drawing.Point(0, 0);
            this.lblCalculating.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCalculating.Name = "lblCalculating";
            this.lblCalculating.Size = new System.Drawing.Size(985, 555);
            this.lblCalculating.TabIndex = 1;
            this.lblCalculating.Tag = "themeable";
            this.lblCalculating.Text = "Drag and drop files here...";
            this.lblCalculating.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SumView
            // 
            this.SumView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.SumView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SumView.ContextMenuStrip = this.helperMenu;
            this.SumView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SumView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.SumView.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SumView.ForeColor = System.Drawing.Color.White;
            this.SumView.Location = new System.Drawing.Point(0, 0);
            this.SumView.Margin = new System.Windows.Forms.Padding(2);
            this.SumView.Name = "SumView";
            this.SumView.Size = new System.Drawing.Size(985, 555);
            this.SumView.TabIndex = 0;
            this.SumView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.sumView_NodeMouseClick);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Hashing";
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.trayMenu.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trayMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.trayMenu.ShowImageMargin = false;
            this.trayMenu.Size = new System.Drawing.Size(101, 52);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restoreToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(100, 24);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.toolStripMenuItem11.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(211, 24);
            this.toolStripMenuItem11.Text = "RIPEMD160";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(987, 629);
            this.Controls.Add(this.botPanel);
            this.Controls.Add(this.topPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(1003, 668);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hashing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.helperMenu.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.botPanel.ResumeLayout(false);
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MoonTree SumView;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel botPanel;
        private System.Windows.Forms.Label lblCalculating;
        private System.Windows.Forms.Button btnSaveJson;
        private System.Windows.Forms.ContextMenuStrip helperMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button btnFindIdenticals;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblversion;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnCancelHashing;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem itemMD5;
        private System.Windows.Forms.ToolStripMenuItem itemSHA1;
        private System.Windows.Forms.ToolStripMenuItem itemSHA256;
        private System.Windows.Forms.ToolStripMenuItem itemSHA384;
        private System.Windows.Forms.ToolStripMenuItem itemSHA512;
        private System.Windows.Forms.ToolStripMenuItem itemCRC32;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem itemRIPEMD160;
    }
}

