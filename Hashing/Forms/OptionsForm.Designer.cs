namespace Hashing
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.okbtn = new System.Windows.Forms.Button();
            this.minimaltheme = new System.Windows.Forms.RadioButton();
            this.carameltheme = new System.Windows.Forms.RadioButton();
            this.limetheme = new System.Windows.Forms.RadioButton();
            this.magmatheme = new System.Windows.Forms.RadioButton();
            this.oceantheme = new System.Windows.Forms.RadioButton();
            this.zergtheme = new System.Windows.Forms.RadioButton();
            this.label27 = new System.Windows.Forms.Label();
            this.lbl88 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox88 = new System.Windows.Forms.PictureBox();
            this.pictureBox86 = new System.Windows.Forms.PictureBox();
            this.radioChinese = new Hashing.MoonRadio();
            this.radioHellenic = new Hashing.MoonRadio();
            this.radioEnglish = new Hashing.MoonRadio();
            this.chkStayOnTop = new Hashing.MoonCheck();
            this.chkSingleInstance = new Hashing.MoonCheck();
            this.chkCRCFormat = new Hashing.MoonCheck();
            this.chkLower = new Hashing.MoonCheck();
            this.chkCRC32 = new Hashing.MoonCheck();
            this.chkHigh = new Hashing.MoonCheck();
            this.chkTray = new Hashing.MoonCheck();
            this.chkSHA384 = new Hashing.MoonCheck();
            this.chkRIPEMD160 = new Hashing.MoonCheck();
            this.chkSHA512 = new Hashing.MoonCheck();
            this.chkSHA256 = new Hashing.MoonCheck();
            this.chkSHA1 = new Hashing.MoonCheck();
            this.chkMD5 = new Hashing.MoonCheck();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox88)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox86)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // okbtn
            // 
            this.okbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.okbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okbtn.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.okbtn.FlatAppearance.BorderSize = 0;
            this.okbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.okbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.okbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okbtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okbtn.ForeColor = System.Drawing.Color.White;
            this.okbtn.Location = new System.Drawing.Point(426, 369);
            this.okbtn.Margin = new System.Windows.Forms.Padding(2);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(147, 31);
            this.okbtn.TabIndex = 70;
            this.okbtn.Tag = "themeable";
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = false;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // minimaltheme
            // 
            this.minimaltheme.AutoSize = true;
            this.minimaltheme.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minimaltheme.ForeColor = System.Drawing.Color.Gray;
            this.minimaltheme.Location = new System.Drawing.Point(114, 110);
            this.minimaltheme.Margin = new System.Windows.Forms.Padding(2);
            this.minimaltheme.Name = "minimaltheme";
            this.minimaltheme.Size = new System.Drawing.Size(86, 25);
            this.minimaltheme.TabIndex = 69;
            this.minimaltheme.Text = "Minimal";
            this.minimaltheme.UseVisualStyleBackColor = true;
            this.minimaltheme.CheckedChanged += new System.EventHandler(this.minimaltheme_CheckedChanged);
            // 
            // carameltheme
            // 
            this.carameltheme.AutoSize = true;
            this.carameltheme.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carameltheme.ForeColor = System.Drawing.Color.DarkOrange;
            this.carameltheme.Location = new System.Drawing.Point(114, 49);
            this.carameltheme.Margin = new System.Windows.Forms.Padding(2);
            this.carameltheme.Name = "carameltheme";
            this.carameltheme.Size = new System.Drawing.Size(87, 25);
            this.carameltheme.TabIndex = 68;
            this.carameltheme.Text = "Caramel";
            this.carameltheme.UseVisualStyleBackColor = true;
            this.carameltheme.CheckedChanged += new System.EventHandler(this.carameltheme_CheckedChanged);
            // 
            // limetheme
            // 
            this.limetheme.AutoSize = true;
            this.limetheme.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.limetheme.ForeColor = System.Drawing.Color.LimeGreen;
            this.limetheme.Location = new System.Drawing.Point(114, 80);
            this.limetheme.Margin = new System.Windows.Forms.Padding(2);
            this.limetheme.Name = "limetheme";
            this.limetheme.Size = new System.Drawing.Size(63, 25);
            this.limetheme.TabIndex = 67;
            this.limetheme.Text = "Lime";
            this.limetheme.UseVisualStyleBackColor = true;
            this.limetheme.CheckedChanged += new System.EventHandler(this.limetheme_CheckedChanged);
            // 
            // magmatheme
            // 
            this.magmatheme.AutoSize = true;
            this.magmatheme.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.magmatheme.ForeColor = System.Drawing.Color.Tomato;
            this.magmatheme.Location = new System.Drawing.Point(22, 80);
            this.magmatheme.Margin = new System.Windows.Forms.Padding(2);
            this.magmatheme.Name = "magmatheme";
            this.magmatheme.Size = new System.Drawing.Size(83, 25);
            this.magmatheme.TabIndex = 66;
            this.magmatheme.Text = "Magma";
            this.magmatheme.UseVisualStyleBackColor = true;
            this.magmatheme.CheckedChanged += new System.EventHandler(this.magmatheme_CheckedChanged);
            // 
            // oceantheme
            // 
            this.oceantheme.AutoSize = true;
            this.oceantheme.Checked = true;
            this.oceantheme.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oceantheme.ForeColor = System.Drawing.Color.DodgerBlue;
            this.oceantheme.Location = new System.Drawing.Point(22, 49);
            this.oceantheme.Margin = new System.Windows.Forms.Padding(2);
            this.oceantheme.Name = "oceantheme";
            this.oceantheme.Size = new System.Drawing.Size(74, 25);
            this.oceantheme.TabIndex = 65;
            this.oceantheme.TabStop = true;
            this.oceantheme.Text = "Ocean";
            this.oceantheme.UseVisualStyleBackColor = true;
            this.oceantheme.CheckedChanged += new System.EventHandler(this.oceantheme_CheckedChanged);
            // 
            // zergtheme
            // 
            this.zergtheme.AutoSize = true;
            this.zergtheme.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zergtheme.ForeColor = System.Drawing.Color.MediumOrchid;
            this.zergtheme.Location = new System.Drawing.Point(22, 110);
            this.zergtheme.Margin = new System.Windows.Forms.Padding(2);
            this.zergtheme.Name = "zergtheme";
            this.zergtheme.Size = new System.Drawing.Size(62, 25);
            this.zergtheme.TabIndex = 64;
            this.zergtheme.Text = "Zerg";
            this.zergtheme.UseVisualStyleBackColor = true;
            this.zergtheme.CheckedChanged += new System.EventHandler(this.zergtheme_CheckedChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label27.Location = new System.Drawing.Point(2, 10);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(153, 21);
            this.label27.TabIndex = 63;
            this.label27.Tag = "themeable";
            this.label27.Text = "Choose your theme";
            // 
            // lbl88
            // 
            this.lbl88.AutoSize = true;
            this.lbl88.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl88.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lbl88.Location = new System.Drawing.Point(10, 167);
            this.lbl88.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl88.Name = "lbl88";
            this.lbl88.Size = new System.Drawing.Size(214, 21);
            this.lbl88.TabIndex = 71;
            this.lbl88.Tag = "themeable";
            this.lbl88.Text = "Choose your desired hashes";
            this.lbl88.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label3.Location = new System.Drawing.Point(2, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 21);
            this.label3.TabIndex = 85;
            this.label3.Tag = "themeable";
            this.label3.Text = "Choose language";
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(27, 113);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(36, 22);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 104;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Click += new System.EventHandler(this.pictureBox7_Click);
            // 
            // pictureBox88
            // 
            this.pictureBox88.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox88.Image")));
            this.pictureBox88.Location = new System.Drawing.Point(27, 81);
            this.pictureBox88.Name = "pictureBox88";
            this.pictureBox88.Size = new System.Drawing.Size(36, 22);
            this.pictureBox88.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox88.TabIndex = 103;
            this.pictureBox88.TabStop = false;
            this.pictureBox88.Click += new System.EventHandler(this.pictureBox88_Click);
            // 
            // pictureBox86
            // 
            this.pictureBox86.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox86.Image")));
            this.pictureBox86.Location = new System.Drawing.Point(27, 50);
            this.pictureBox86.Name = "pictureBox86";
            this.pictureBox86.Size = new System.Drawing.Size(36, 22);
            this.pictureBox86.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox86.TabIndex = 102;
            this.pictureBox86.TabStop = false;
            this.pictureBox86.Click += new System.EventHandler(this.pictureBox86_Click);
            // 
            // radioChinese
            // 
            this.radioChinese.AutoSize = true;
            this.radioChinese.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChinese.Location = new System.Drawing.Point(68, 111);
            this.radioChinese.Name = "radioChinese";
            this.radioChinese.Size = new System.Drawing.Size(96, 25);
            this.radioChinese.TabIndex = 107;
            this.radioChinese.TabStop = true;
            this.radioChinese.Text = "简体中文";
            this.radioChinese.UseVisualStyleBackColor = true;
            this.radioChinese.Click += new System.EventHandler(this.radioChinese_Click);
            // 
            // radioHellenic
            // 
            this.radioHellenic.AutoSize = true;
            this.radioHellenic.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioHellenic.Location = new System.Drawing.Point(68, 80);
            this.radioHellenic.Name = "radioHellenic";
            this.radioHellenic.Size = new System.Drawing.Size(94, 25);
            this.radioHellenic.TabIndex = 106;
            this.radioHellenic.TabStop = true;
            this.radioHellenic.Text = "Ελληνικά";
            this.radioHellenic.UseVisualStyleBackColor = true;
            this.radioHellenic.Click += new System.EventHandler(this.radioHellenic_Click);
            // 
            // radioEnglish
            // 
            this.radioEnglish.AutoSize = true;
            this.radioEnglish.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioEnglish.Location = new System.Drawing.Point(68, 49);
            this.radioEnglish.Name = "radioEnglish";
            this.radioEnglish.Size = new System.Drawing.Size(79, 25);
            this.radioEnglish.TabIndex = 105;
            this.radioEnglish.TabStop = true;
            this.radioEnglish.Text = "English";
            this.radioEnglish.UseVisualStyleBackColor = true;
            this.radioEnglish.Click += new System.EventHandler(this.radioEnglish_Click);
            // 
            // chkStayOnTop
            // 
            this.chkStayOnTop.AutoSize = true;
            this.chkStayOnTop.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStayOnTop.ForeColor = System.Drawing.Color.White;
            this.chkStayOnTop.Location = new System.Drawing.Point(283, 253);
            this.chkStayOnTop.Margin = new System.Windows.Forms.Padding(2);
            this.chkStayOnTop.Name = "chkStayOnTop";
            this.chkStayOnTop.Size = new System.Drawing.Size(113, 25);
            this.chkStayOnTop.TabIndex = 84;
            this.chkStayOnTop.Text = "Stay on top";
            this.chkStayOnTop.UseVisualStyleBackColor = true;
            this.chkStayOnTop.CheckedChanged += new System.EventHandler(this.chkStayOnTop_CheckedChanged);
            // 
            // chkSingleInstance
            // 
            this.chkSingleInstance.AutoSize = true;
            this.chkSingleInstance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSingleInstance.ForeColor = System.Drawing.Color.White;
            this.chkSingleInstance.Location = new System.Drawing.Point(283, 225);
            this.chkSingleInstance.Margin = new System.Windows.Forms.Padding(2);
            this.chkSingleInstance.Name = "chkSingleInstance";
            this.chkSingleInstance.Size = new System.Drawing.Size(188, 25);
            this.chkSingleInstance.TabIndex = 83;
            this.chkSingleInstance.Text = "Enable single instance";
            this.chkSingleInstance.UseVisualStyleBackColor = true;
            this.chkSingleInstance.CheckedChanged += new System.EventHandler(this.chkSingleInstance_CheckedChanged);
            // 
            // chkCRCFormat
            // 
            this.chkCRCFormat.AutoSize = true;
            this.chkCRCFormat.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCRCFormat.ForeColor = System.Drawing.Color.White;
            this.chkCRCFormat.Location = new System.Drawing.Point(283, 196);
            this.chkCRCFormat.Margin = new System.Windows.Forms.Padding(2);
            this.chkCRCFormat.Name = "chkCRCFormat";
            this.chkCRCFormat.Size = new System.Drawing.Size(207, 25);
            this.chkCRCFormat.TabIndex = 82;
            this.chkCRCFormat.Text = "Prefer CRC32 to decimal";
            this.chkCRCFormat.UseVisualStyleBackColor = true;
            this.chkCRCFormat.CheckedChanged += new System.EventHandler(this.chkCRCFormat_CheckedChanged);
            // 
            // chkLower
            // 
            this.chkLower.AutoSize = true;
            this.chkLower.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLower.ForeColor = System.Drawing.Color.White;
            this.chkLower.Location = new System.Drawing.Point(283, 167);
            this.chkLower.Margin = new System.Windows.Forms.Padding(2);
            this.chkLower.Name = "chkLower";
            this.chkLower.Size = new System.Drawing.Size(169, 25);
            this.chkLower.TabIndex = 81;
            this.chkLower.Text = "Prefer lower casing";
            this.chkLower.UseVisualStyleBackColor = true;
            this.chkLower.CheckedChanged += new System.EventHandler(this.chkLower_CheckedChanged);
            // 
            // chkCRC32
            // 
            this.chkCRC32.AutoSize = true;
            this.chkCRC32.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCRC32.ForeColor = System.Drawing.Color.White;
            this.chkCRC32.Location = new System.Drawing.Point(122, 205);
            this.chkCRC32.Margin = new System.Windows.Forms.Padding(2);
            this.chkCRC32.Name = "chkCRC32";
            this.chkCRC32.Size = new System.Drawing.Size(77, 25);
            this.chkCRC32.TabIndex = 80;
            this.chkCRC32.Text = "CRC32";
            this.chkCRC32.UseVisualStyleBackColor = true;
            this.chkCRC32.CheckedChanged += new System.EventHandler(this.chkCRC32_CheckedChanged);
            // 
            // chkHigh
            // 
            this.chkHigh.AutoSize = true;
            this.chkHigh.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHigh.ForeColor = System.Drawing.Color.White;
            this.chkHigh.Location = new System.Drawing.Point(283, 311);
            this.chkHigh.Margin = new System.Windows.Forms.Padding(2);
            this.chkHigh.Name = "chkHigh";
            this.chkHigh.Size = new System.Drawing.Size(171, 25);
            this.chkHigh.TabIndex = 79;
            this.chkHigh.Text = "Enable high priority";
            this.chkHigh.UseVisualStyleBackColor = true;
            this.chkHigh.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chkTray
            // 
            this.chkTray.AutoSize = true;
            this.chkTray.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTray.ForeColor = System.Drawing.Color.White;
            this.chkTray.Location = new System.Drawing.Point(283, 282);
            this.chkTray.Margin = new System.Windows.Forms.Padding(2);
            this.chkTray.Name = "chkTray";
            this.chkTray.Size = new System.Drawing.Size(156, 25);
            this.chkTray.TabIndex = 78;
            this.chkTray.Text = "Close to tray icon";
            this.chkTray.UseVisualStyleBackColor = true;
            this.chkTray.CheckedChanged += new System.EventHandler(this.chkTray_CheckedChanged);
            // 
            // chkSHA384
            // 
            this.chkSHA384.AutoSize = true;
            this.chkSHA384.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA384.ForeColor = System.Drawing.Color.White;
            this.chkSHA384.Location = new System.Drawing.Point(30, 266);
            this.chkSHA384.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA384.Name = "chkSHA384";
            this.chkSHA384.Size = new System.Drawing.Size(88, 25);
            this.chkSHA384.TabIndex = 77;
            this.chkSHA384.Text = "SHA384";
            this.chkSHA384.UseVisualStyleBackColor = true;
            this.chkSHA384.CheckedChanged += new System.EventHandler(this.chkSHA384_CheckedChanged);
            // 
            // chkRIPEMD160
            // 
            this.chkRIPEMD160.AutoSize = true;
            this.chkRIPEMD160.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRIPEMD160.ForeColor = System.Drawing.Color.White;
            this.chkRIPEMD160.Location = new System.Drawing.Point(30, 295);
            this.chkRIPEMD160.Margin = new System.Windows.Forms.Padding(2);
            this.chkRIPEMD160.Name = "chkRIPEMD160";
            this.chkRIPEMD160.Size = new System.Drawing.Size(111, 25);
            this.chkRIPEMD160.TabIndex = 76;
            this.chkRIPEMD160.Text = "RIPEMD160";
            this.chkRIPEMD160.UseVisualStyleBackColor = true;
            this.chkRIPEMD160.CheckedChanged += new System.EventHandler(this.chkRIPEMD160_CheckedChanged);
            // 
            // chkSHA512
            // 
            this.chkSHA512.AutoSize = true;
            this.chkSHA512.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA512.ForeColor = System.Drawing.Color.White;
            this.chkSHA512.Location = new System.Drawing.Point(122, 266);
            this.chkSHA512.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA512.Name = "chkSHA512";
            this.chkSHA512.Size = new System.Drawing.Size(85, 25);
            this.chkSHA512.TabIndex = 75;
            this.chkSHA512.Text = "SHA512";
            this.chkSHA512.UseVisualStyleBackColor = true;
            this.chkSHA512.CheckedChanged += new System.EventHandler(this.chkSHA512_CheckedChanged);
            // 
            // chkSHA256
            // 
            this.chkSHA256.AutoSize = true;
            this.chkSHA256.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA256.ForeColor = System.Drawing.Color.White;
            this.chkSHA256.Location = new System.Drawing.Point(122, 236);
            this.chkSHA256.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA256.Name = "chkSHA256";
            this.chkSHA256.Size = new System.Drawing.Size(88, 25);
            this.chkSHA256.TabIndex = 74;
            this.chkSHA256.Text = "SHA256";
            this.chkSHA256.UseVisualStyleBackColor = true;
            this.chkSHA256.CheckedChanged += new System.EventHandler(this.chkSHA256_CheckedChanged);
            // 
            // chkSHA1
            // 
            this.chkSHA1.AutoSize = true;
            this.chkSHA1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA1.ForeColor = System.Drawing.Color.White;
            this.chkSHA1.Location = new System.Drawing.Point(30, 236);
            this.chkSHA1.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA1.Name = "chkSHA1";
            this.chkSHA1.Size = new System.Drawing.Size(67, 25);
            this.chkSHA1.TabIndex = 73;
            this.chkSHA1.Text = "SHA1";
            this.chkSHA1.UseVisualStyleBackColor = true;
            this.chkSHA1.CheckedChanged += new System.EventHandler(this.chkSHA1_CheckedChanged);
            // 
            // chkMD5
            // 
            this.chkMD5.AutoSize = true;
            this.chkMD5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMD5.ForeColor = System.Drawing.Color.White;
            this.chkMD5.Location = new System.Drawing.Point(30, 205);
            this.chkMD5.Margin = new System.Windows.Forms.Padding(2);
            this.chkMD5.Name = "chkMD5";
            this.chkMD5.Size = new System.Drawing.Size(64, 25);
            this.chkMD5.TabIndex = 72;
            this.chkMD5.Text = "MD5";
            this.chkMD5.UseVisualStyleBackColor = true;
            this.chkMD5.CheckedChanged += new System.EventHandler(this.chkMD5_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.zergtheme);
            this.panel1.Controls.Add(this.oceantheme);
            this.panel1.Controls.Add(this.magmatheme);
            this.panel1.Controls.Add(this.limetheme);
            this.panel1.Controls.Add(this.carameltheme);
            this.panel1.Controls.Add(this.minimaltheme);
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 156);
            this.panel1.TabIndex = 108;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.pictureBox86);
            this.panel2.Controls.Add(this.radioChinese);
            this.panel2.Controls.Add(this.pictureBox88);
            this.panel2.Controls.Add(this.radioHellenic);
            this.panel2.Controls.Add(this.pictureBox7);
            this.panel2.Controls.Add(this.radioEnglish);
            this.panel2.Location = new System.Drawing.Point(271, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(181, 150);
            this.panel2.TabIndex = 109;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okbtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.CancelButton = this.okbtn;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkStayOnTop);
            this.Controls.Add(this.chkSingleInstance);
            this.Controls.Add(this.chkCRCFormat);
            this.Controls.Add(this.chkLower);
            this.Controls.Add(this.chkCRC32);
            this.Controls.Add(this.chkHigh);
            this.Controls.Add(this.chkTray);
            this.Controls.Add(this.chkSHA384);
            this.Controls.Add(this.chkRIPEMD160);
            this.Controls.Add(this.chkSHA512);
            this.Controls.Add(this.chkSHA256);
            this.Controls.Add(this.chkSHA1);
            this.Controls.Add(this.chkMD5);
            this.Controls.Add(this.lbl88);
            this.Controls.Add(this.okbtn);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox88)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox86)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.RadioButton minimaltheme;
        private System.Windows.Forms.RadioButton carameltheme;
        private System.Windows.Forms.RadioButton limetheme;
        private System.Windows.Forms.RadioButton magmatheme;
        private System.Windows.Forms.RadioButton oceantheme;
        private System.Windows.Forms.RadioButton zergtheme;
        private System.Windows.Forms.Label label27;
        private MoonCheck chkRIPEMD160;
        private MoonCheck chkSHA512;
        private MoonCheck chkSHA256;
        private MoonCheck chkSHA1;
        private MoonCheck chkMD5;
        private System.Windows.Forms.Label lbl88;
        private MoonCheck chkSHA384;
        private MoonCheck chkTray;
        private MoonCheck chkHigh;
        private MoonCheck chkCRC32;
        private MoonCheck chkLower;
        private MoonCheck chkCRCFormat;
        private MoonCheck chkSingleInstance;
        private MoonCheck chkStayOnTop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox88;
        private System.Windows.Forms.PictureBox pictureBox86;
        private MoonRadio radioEnglish;
        private MoonRadio radioHellenic;
        private MoonRadio radioChinese;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}