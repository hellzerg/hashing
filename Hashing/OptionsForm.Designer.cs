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
            this.okbtn = new System.Windows.Forms.Button();
            this.minimaltheme = new System.Windows.Forms.RadioButton();
            this.carameltheme = new System.Windows.Forms.RadioButton();
            this.limetheme = new System.Windows.Forms.RadioButton();
            this.magmatheme = new System.Windows.Forms.RadioButton();
            this.oceantheme = new System.Windows.Forms.RadioButton();
            this.zergtheme = new System.Windows.Forms.RadioButton();
            this.label27 = new System.Windows.Forms.Label();
            this.lbl88 = new System.Windows.Forms.Label();
            this.chkStayOnTop = new Hashing.ColoredCheckBox();
            this.chkSingleInstance = new Hashing.ColoredCheckBox();
            this.chkCRCFormat = new Hashing.ColoredCheckBox();
            this.chkLower = new Hashing.ColoredCheckBox();
            this.chkCRC32 = new Hashing.ColoredCheckBox();
            this.chkHigh = new Hashing.ColoredCheckBox();
            this.chkTray = new Hashing.ColoredCheckBox();
            this.chkSHA384 = new Hashing.ColoredCheckBox();
            this.chkRIPEMD160 = new Hashing.ColoredCheckBox();
            this.chkSHA512 = new Hashing.ColoredCheckBox();
            this.chkSHA256 = new Hashing.ColoredCheckBox();
            this.chkSHA1 = new Hashing.ColoredCheckBox();
            this.chkMD5 = new Hashing.ColoredCheckBox();
            this.SuspendLayout();
            // 
            // okbtn
            // 
            this.okbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.okbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okbtn.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.okbtn.FlatAppearance.BorderSize = 0;
            this.okbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.okbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.okbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okbtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okbtn.ForeColor = System.Drawing.Color.White;
            this.okbtn.Location = new System.Drawing.Point(379, 504);
            this.okbtn.Margin = new System.Windows.Forms.Padding(2);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(71, 31);
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
            this.minimaltheme.Location = new System.Drawing.Point(122, 108);
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
            this.carameltheme.Location = new System.Drawing.Point(122, 47);
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
            this.limetheme.Location = new System.Drawing.Point(122, 78);
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
            this.magmatheme.Location = new System.Drawing.Point(30, 78);
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
            this.oceantheme.Location = new System.Drawing.Point(30, 47);
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
            this.zergtheme.Location = new System.Drawing.Point(30, 108);
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
            this.label27.Location = new System.Drawing.Point(10, 8);
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
            this.lbl88.Location = new System.Drawing.Point(10, 152);
            this.lbl88.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl88.Name = "lbl88";
            this.lbl88.Size = new System.Drawing.Size(214, 21);
            this.lbl88.TabIndex = 71;
            this.lbl88.Tag = "themeable";
            this.lbl88.Text = "Choose your desired hashes";
            this.lbl88.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkStayOnTop
            // 
            this.chkStayOnTop.AutoSize = true;
            this.chkStayOnTop.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStayOnTop.ForeColor = System.Drawing.Color.White;
            this.chkStayOnTop.Location = new System.Drawing.Point(15, 410);
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
            this.chkSingleInstance.Location = new System.Drawing.Point(15, 382);
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
            this.chkCRCFormat.Location = new System.Drawing.Point(15, 353);
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
            this.chkLower.Location = new System.Drawing.Point(15, 324);
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
            this.chkCRC32.Location = new System.Drawing.Point(122, 190);
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
            this.chkHigh.Location = new System.Drawing.Point(15, 468);
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
            this.chkTray.Location = new System.Drawing.Point(15, 439);
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
            this.chkSHA384.Location = new System.Drawing.Point(30, 251);
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
            this.chkRIPEMD160.Location = new System.Drawing.Point(30, 280);
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
            this.chkSHA512.Location = new System.Drawing.Point(122, 251);
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
            this.chkSHA256.Location = new System.Drawing.Point(122, 221);
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
            this.chkSHA1.Location = new System.Drawing.Point(30, 221);
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
            this.chkMD5.Location = new System.Drawing.Point(30, 190);
            this.chkMD5.Margin = new System.Windows.Forms.Padding(2);
            this.chkMD5.Name = "chkMD5";
            this.chkMD5.Size = new System.Drawing.Size(64, 25);
            this.chkMD5.TabIndex = 72;
            this.chkMD5.Text = "MD5";
            this.chkMD5.UseVisualStyleBackColor = true;
            this.chkMD5.CheckedChanged += new System.EventHandler(this.chkMD5_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okbtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.CancelButton = this.okbtn;
            this.ClientSize = new System.Drawing.Size(459, 544);
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
            this.Controls.Add(this.minimaltheme);
            this.Controls.Add(this.carameltheme);
            this.Controls.Add(this.limetheme);
            this.Controls.Add(this.magmatheme);
            this.Controls.Add(this.oceantheme);
            this.Controls.Add(this.zergtheme);
            this.Controls.Add(this.label27);
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
        private ColoredCheckBox chkRIPEMD160;
        private ColoredCheckBox chkSHA512;
        private ColoredCheckBox chkSHA256;
        private ColoredCheckBox chkSHA1;
        private ColoredCheckBox chkMD5;
        private System.Windows.Forms.Label lbl88;
        private ColoredCheckBox chkSHA384;
        private ColoredCheckBox chkTray;
        private ColoredCheckBox chkHigh;
        private ColoredCheckBox chkCRC32;
        private ColoredCheckBox chkLower;
        private ColoredCheckBox chkCRCFormat;
        private ColoredCheckBox chkSingleInstance;
        private ColoredCheckBox chkStayOnTop;
    }
}