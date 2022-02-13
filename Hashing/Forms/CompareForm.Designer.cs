namespace Hashing
{
    partial class CompareForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExpected = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblNoMatch = new System.Windows.Forms.Label();
            this.btnPaste = new System.Windows.Forms.Button();
            this.chkRemove = new Hashing.MoonCheck();
            this.resultBox = new Hashing.MoonList();
            this.chkSHA35 = new Hashing.MoonCheck();
            this.chkSHA33 = new Hashing.MoonCheck();
            this.chkSHA32 = new Hashing.MoonCheck();
            this.chkCRC32 = new Hashing.MoonCheck();
            this.chkSHA512 = new Hashing.MoonCheck();
            this.chkSHA384 = new Hashing.MoonCheck();
            this.chkRIPEMD160 = new Hashing.MoonCheck();
            this.chkSHA256 = new Hashing.MoonCheck();
            this.chkSHA1 = new Hashing.MoonCheck();
            this.chkMD5 = new Hashing.MoonCheck();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkSHA35);
            this.panel1.Controls.Add(this.chkSHA33);
            this.panel1.Controls.Add(this.chkSHA32);
            this.panel1.Controls.Add(this.chkCRC32);
            this.panel1.Controls.Add(this.chkSHA512);
            this.panel1.Controls.Add(this.chkSHA384);
            this.panel1.Controls.Add(this.chkRIPEMD160);
            this.panel1.Controls.Add(this.chkSHA256);
            this.panel1.Controls.Add(this.chkSHA1);
            this.panel1.Controls.Add(this.chkMD5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 32);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(6, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 21);
            this.label1.TabIndex = 5;
            this.label1.Tag = "themeable";
            this.label1.Text = "Expected hash:";
            // 
            // txtExpected
            // 
            this.txtExpected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.txtExpected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExpected.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtExpected.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExpected.ForeColor = System.Drawing.Color.White;
            this.txtExpected.Location = new System.Drawing.Point(10, 61);
            this.txtExpected.Margin = new System.Windows.Forms.Padding(2);
            this.txtExpected.Name = "txtExpected";
            this.txtExpected.Size = new System.Drawing.Size(845, 27);
            this.txtExpected.TabIndex = 6;
            this.txtExpected.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExpected_KeyDown);
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCompare.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatAppearance.BorderSize = 0;
            this.btnCompare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompare.ForeColor = System.Drawing.Color.White;
            this.btnCompare.Location = new System.Drawing.Point(704, 92);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(151, 27);
            this.btnCompare.TabIndex = 74;
            this.btnCompare.Tag = "themeable";
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = false;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label2.Location = new System.Drawing.Point(6, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 21);
            this.label2.TabIndex = 75;
            this.label2.Tag = "themeable";
            this.label2.Text = "Results:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblNoMatch);
            this.panel2.Controls.Add(this.resultBox);
            this.panel2.Location = new System.Drawing.Point(10, 135);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(845, 391);
            this.panel2.TabIndex = 77;
            // 
            // lblNoMatch
            // 
            this.lblNoMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoMatch.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoMatch.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblNoMatch.Location = new System.Drawing.Point(0, 0);
            this.lblNoMatch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNoMatch.Name = "lblNoMatch";
            this.lblNoMatch.Size = new System.Drawing.Size(843, 389);
            this.lblNoMatch.TabIndex = 77;
            this.lblNoMatch.Tag = "themeable";
            this.lblNoMatch.Text = "No files matching";
            this.lblNoMatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPaste.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatAppearance.BorderSize = 0;
            this.btnPaste.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste.ForeColor = System.Drawing.Color.White;
            this.btnPaste.Location = new System.Drawing.Point(549, 92);
            this.btnPaste.Margin = new System.Windows.Forms.Padding(2);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(151, 27);
            this.btnPaste.TabIndex = 79;
            this.btnPaste.Tag = "themeable";
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = false;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // chkRemove
            // 
            this.chkRemove.AutoSize = true;
            this.chkRemove.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemove.Location = new System.Drawing.Point(10, 89);
            this.chkRemove.Margin = new System.Windows.Forms.Padding(2);
            this.chkRemove.Name = "chkRemove";
            this.chkRemove.Size = new System.Drawing.Size(124, 23);
            this.chkRemove.TabIndex = 78;
            this.chkRemove.Text = "Remove dashes";
            this.chkRemove.UseVisualStyleBackColor = true;
            this.chkRemove.CheckedChanged += new System.EventHandler(this.chkRemove_CheckedChanged);
            // 
            // resultBox
            // 
            this.resultBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.resultBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.resultBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultBox.ForeColor = System.Drawing.Color.White;
            this.resultBox.FormattingEnabled = true;
            this.resultBox.HorizontalScrollbar = true;
            this.resultBox.ItemHeight = 21;
            this.resultBox.Location = new System.Drawing.Point(0, 0);
            this.resultBox.Margin = new System.Windows.Forms.Padding(2);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(843, 389);
            this.resultBox.TabIndex = 76;
            this.resultBox.DoubleClick += new System.EventHandler(this.resultBox_DoubleClick);
            // 
            // chkSHA35
            // 
            this.chkSHA35.AutoSize = true;
            this.chkSHA35.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA35.Location = new System.Drawing.Point(752, 4);
            this.chkSHA35.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA35.Name = "chkSHA35";
            this.chkSHA35.Size = new System.Drawing.Size(91, 23);
            this.chkSHA35.TabIndex = 9;
            this.chkSHA35.Text = "SHA3-512";
            this.chkSHA35.UseVisualStyleBackColor = true;
            this.chkSHA35.CheckedChanged += new System.EventHandler(this.chkSHA35_CheckedChanged);
            // 
            // chkSHA33
            // 
            this.chkSHA33.AutoSize = true;
            this.chkSHA33.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA33.Location = new System.Drawing.Point(655, 4);
            this.chkSHA33.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA33.Name = "chkSHA33";
            this.chkSHA33.Size = new System.Drawing.Size(93, 23);
            this.chkSHA33.TabIndex = 8;
            this.chkSHA33.Text = "SHA3-384";
            this.chkSHA33.UseVisualStyleBackColor = true;
            this.chkSHA33.CheckedChanged += new System.EventHandler(this.chkSHA33_CheckedChanged);
            // 
            // chkSHA32
            // 
            this.chkSHA32.AutoSize = true;
            this.chkSHA32.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA32.Location = new System.Drawing.Point(558, 4);
            this.chkSHA32.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA32.Name = "chkSHA32";
            this.chkSHA32.Size = new System.Drawing.Size(93, 23);
            this.chkSHA32.TabIndex = 7;
            this.chkSHA32.Text = "SHA3-256";
            this.chkSHA32.UseVisualStyleBackColor = true;
            this.chkSHA32.CheckedChanged += new System.EventHandler(this.chkSHA32_CheckedChanged);
            // 
            // chkCRC32
            // 
            this.chkCRC32.AutoSize = true;
            this.chkCRC32.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCRC32.Location = new System.Drawing.Point(379, 4);
            this.chkCRC32.Margin = new System.Windows.Forms.Padding(2);
            this.chkCRC32.Name = "chkCRC32";
            this.chkCRC32.Size = new System.Drawing.Size(71, 23);
            this.chkCRC32.TabIndex = 6;
            this.chkCRC32.Text = "CRC32";
            this.chkCRC32.UseVisualStyleBackColor = true;
            this.chkCRC32.CheckedChanged += new System.EventHandler(this.chkCRC32_CheckedChanged);
            // 
            // chkSHA512
            // 
            this.chkSHA512.AutoSize = true;
            this.chkSHA512.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA512.Location = new System.Drawing.Point(299, 4);
            this.chkSHA512.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA512.Name = "chkSHA512";
            this.chkSHA512.Size = new System.Drawing.Size(77, 23);
            this.chkSHA512.TabIndex = 5;
            this.chkSHA512.Text = "SHA512";
            this.chkSHA512.UseVisualStyleBackColor = true;
            this.chkSHA512.CheckedChanged += new System.EventHandler(this.chkSHA512_CheckedChanged);
            // 
            // chkSHA384
            // 
            this.chkSHA384.AutoSize = true;
            this.chkSHA384.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA384.Location = new System.Drawing.Point(217, 4);
            this.chkSHA384.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA384.Name = "chkSHA384";
            this.chkSHA384.Size = new System.Drawing.Size(79, 23);
            this.chkSHA384.TabIndex = 4;
            this.chkSHA384.Text = "SHA384";
            this.chkSHA384.UseVisualStyleBackColor = true;
            this.chkSHA384.CheckedChanged += new System.EventHandler(this.chkSHA384_CheckedChanged);
            // 
            // chkRIPEMD160
            // 
            this.chkRIPEMD160.AutoSize = true;
            this.chkRIPEMD160.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRIPEMD160.Location = new System.Drawing.Point(453, 4);
            this.chkRIPEMD160.Margin = new System.Windows.Forms.Padding(2);
            this.chkRIPEMD160.Name = "chkRIPEMD160";
            this.chkRIPEMD160.Size = new System.Drawing.Size(101, 23);
            this.chkRIPEMD160.TabIndex = 3;
            this.chkRIPEMD160.Text = "RIPEMD160";
            this.chkRIPEMD160.UseVisualStyleBackColor = true;
            this.chkRIPEMD160.CheckedChanged += new System.EventHandler(this.chkRIPEMD160_CheckedChanged);
            // 
            // chkSHA256
            // 
            this.chkSHA256.AutoSize = true;
            this.chkSHA256.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA256.Location = new System.Drawing.Point(135, 4);
            this.chkSHA256.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA256.Name = "chkSHA256";
            this.chkSHA256.Size = new System.Drawing.Size(79, 23);
            this.chkSHA256.TabIndex = 2;
            this.chkSHA256.Text = "SHA256";
            this.chkSHA256.UseVisualStyleBackColor = true;
            this.chkSHA256.CheckedChanged += new System.EventHandler(this.chkSHA256_CheckedChanged);
            // 
            // chkSHA1
            // 
            this.chkSHA1.AutoSize = true;
            this.chkSHA1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSHA1.Location = new System.Drawing.Point(71, 4);
            this.chkSHA1.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA1.Name = "chkSHA1";
            this.chkSHA1.Size = new System.Drawing.Size(61, 23);
            this.chkSHA1.TabIndex = 1;
            this.chkSHA1.Text = "SHA1";
            this.chkSHA1.UseVisualStyleBackColor = true;
            this.chkSHA1.CheckedChanged += new System.EventHandler(this.chkSHA1_CheckedChanged);
            // 
            // chkMD5
            // 
            this.chkMD5.AutoSize = true;
            this.chkMD5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.chkMD5.ForeColor = System.Drawing.Color.White;
            this.chkMD5.Location = new System.Drawing.Point(9, 4);
            this.chkMD5.Margin = new System.Windows.Forms.Padding(2);
            this.chkMD5.Name = "chkMD5";
            this.chkMD5.Size = new System.Drawing.Size(59, 23);
            this.chkMD5.TabIndex = 0;
            this.chkMD5.Tag = "";
            this.chkMD5.Text = "MD5";
            this.chkMD5.UseVisualStyleBackColor = true;
            this.chkMD5.CheckedChanged += new System.EventHandler(this.chkMD5_CheckedChanged);
            // 
            // CompareForm
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(864, 535);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.chkRemove);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.txtExpected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.Name = "CompareForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CompareForm_FormClosing);
            this.Load += new System.EventHandler(this.CompareForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MoonCheck chkSHA512;
        private MoonCheck chkSHA384;
        private MoonCheck chkRIPEMD160;
        private MoonCheck chkSHA256;
        private MoonCheck chkSHA1;
        private MoonCheck chkMD5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExpected;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label2;
        private MoonList resultBox;
        private System.Windows.Forms.Panel panel2;
        private MoonCheck chkRemove;
        private MoonCheck chkCRC32;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Label lblNoMatch;
        private MoonCheck chkSHA35;
        private MoonCheck chkSHA33;
        private MoonCheck chkSHA32;
    }
}