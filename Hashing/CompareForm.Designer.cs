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
            this.chkCRC32 = new ColoredRadioButton();
            this.chkSHA512 = new ColoredRadioButton();
            this.chkSHA384 = new ColoredRadioButton();
            this.chkRIPEMD160 = new ColoredRadioButton();
            this.chkSHA256 = new ColoredRadioButton();
            this.chkSHA1 = new ColoredRadioButton();
            this.chkMD5 = new ColoredRadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExpected = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.resultBox = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblNoMatch = new System.Windows.Forms.Label();
            this.chkRemove = new ColoredCheckBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.panel1.Size = new System.Drawing.Size(894, 40);
            this.panel1.TabIndex = 4;
            // 
            // chkCRC32
            // 
            this.chkCRC32.AutoSize = true;
            this.chkCRC32.Location = new System.Drawing.Point(416, 9);
            this.chkCRC32.Margin = new System.Windows.Forms.Padding(2);
            this.chkCRC32.Name = "chkCRC32";
            this.chkCRC32.Size = new System.Drawing.Size(73, 24);
            this.chkCRC32.TabIndex = 6;
            this.chkCRC32.Text = "CRC32";
            this.chkCRC32.UseVisualStyleBackColor = true;
            this.chkCRC32.CheckedChanged += new System.EventHandler(this.chkCRC32_CheckedChanged);
            // 
            // chkSHA512
            // 
            this.chkSHA512.AutoSize = true;
            this.chkSHA512.Location = new System.Drawing.Point(326, 9);
            this.chkSHA512.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA512.Name = "chkSHA512";
            this.chkSHA512.Size = new System.Drawing.Size(81, 24);
            this.chkSHA512.TabIndex = 5;
            this.chkSHA512.Text = "SHA512";
            this.chkSHA512.UseVisualStyleBackColor = true;
            this.chkSHA512.CheckedChanged += new System.EventHandler(this.chkSHA512_CheckedChanged);
            // 
            // chkSHA384
            // 
            this.chkSHA384.AutoSize = true;
            this.chkSHA384.Location = new System.Drawing.Point(238, 9);
            this.chkSHA384.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA384.Name = "chkSHA384";
            this.chkSHA384.Size = new System.Drawing.Size(84, 24);
            this.chkSHA384.TabIndex = 4;
            this.chkSHA384.Text = "SHA384";
            this.chkSHA384.UseVisualStyleBackColor = true;
            this.chkSHA384.CheckedChanged += new System.EventHandler(this.chkSHA384_CheckedChanged);
            // 
            // chkRIPEMD160
            // 
            this.chkRIPEMD160.AutoSize = true;
            this.chkRIPEMD160.Location = new System.Drawing.Point(496, 9);
            this.chkRIPEMD160.Margin = new System.Windows.Forms.Padding(2);
            this.chkRIPEMD160.Name = "chkRIPEMD160";
            this.chkRIPEMD160.Size = new System.Drawing.Size(107, 24);
            this.chkRIPEMD160.TabIndex = 3;
            this.chkRIPEMD160.Text = "RIPEMD160";
            this.chkRIPEMD160.UseVisualStyleBackColor = true;
            this.chkRIPEMD160.CheckedChanged += new System.EventHandler(this.chkRIPEMD160_CheckedChanged);
            // 
            // chkSHA256
            // 
            this.chkSHA256.AutoSize = true;
            this.chkSHA256.Location = new System.Drawing.Point(151, 9);
            this.chkSHA256.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA256.Name = "chkSHA256";
            this.chkSHA256.Size = new System.Drawing.Size(83, 24);
            this.chkSHA256.TabIndex = 2;
            this.chkSHA256.Text = "SHA256";
            this.chkSHA256.UseVisualStyleBackColor = true;
            this.chkSHA256.CheckedChanged += new System.EventHandler(this.chkSHA256_CheckedChanged);
            // 
            // chkSHA1
            // 
            this.chkSHA1.AutoSize = true;
            this.chkSHA1.Location = new System.Drawing.Point(80, 9);
            this.chkSHA1.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA1.Name = "chkSHA1";
            this.chkSHA1.Size = new System.Drawing.Size(65, 24);
            this.chkSHA1.TabIndex = 1;
            this.chkSHA1.Text = "SHA1";
            this.chkSHA1.UseVisualStyleBackColor = true;
            this.chkSHA1.CheckedChanged += new System.EventHandler(this.chkSHA1_CheckedChanged);
            // 
            // chkMD5
            // 
            this.chkMD5.AutoSize = true;
            this.chkMD5.Checked = true;
            this.chkMD5.Location = new System.Drawing.Point(11, 9);
            this.chkMD5.Margin = new System.Windows.Forms.Padding(2);
            this.chkMD5.Name = "chkMD5";
            this.chkMD5.Size = new System.Drawing.Size(63, 24);
            this.chkMD5.TabIndex = 0;
            this.chkMD5.TabStop = true;
            this.chkMD5.Text = "MD5";
            this.chkMD5.UseVisualStyleBackColor = true;
            this.chkMD5.CheckedChanged += new System.EventHandler(this.chkMD5_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(8, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 28);
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
            this.txtExpected.Location = new System.Drawing.Point(12, 76);
            this.txtExpected.Margin = new System.Windows.Forms.Padding(2);
            this.txtExpected.Name = "txtExpected";
            this.txtExpected.Size = new System.Drawing.Size(697, 32);
            this.txtExpected.TabIndex = 6;
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCompare.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnCompare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCompare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompare.ForeColor = System.Drawing.Color.White;
            this.btnCompare.Location = new System.Drawing.Point(801, 76);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(81, 34);
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
            this.label2.Location = new System.Drawing.Point(8, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 28);
            this.label2.TabIndex = 75;
            this.label2.Tag = "themeable";
            this.label2.Text = "Results:";
            // 
            // resultBox
            // 
            this.resultBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.resultBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultBox.ForeColor = System.Drawing.Color.White;
            this.resultBox.FormattingEnabled = true;
            this.resultBox.HorizontalScrollbar = true;
            this.resultBox.ItemHeight = 28;
            this.resultBox.Location = new System.Drawing.Point(0, 0);
            this.resultBox.Margin = new System.Windows.Forms.Padding(2);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(868, 434);
            this.resultBox.TabIndex = 76;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblNoMatch);
            this.panel2.Controls.Add(this.resultBox);
            this.panel2.Location = new System.Drawing.Point(12, 169);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(870, 436);
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
            this.lblNoMatch.Size = new System.Drawing.Size(868, 434);
            this.lblNoMatch.TabIndex = 77;
            this.lblNoMatch.Tag = "themeable";
            this.lblNoMatch.Text = "No files matching";
            this.lblNoMatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkRemove
            // 
            this.chkRemove.AutoSize = true;
            this.chkRemove.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemove.Location = new System.Drawing.Point(12, 111);
            this.chkRemove.Margin = new System.Windows.Forms.Padding(2);
            this.chkRemove.Name = "chkRemove";
            this.chkRemove.Size = new System.Drawing.Size(152, 27);
            this.chkRemove.TabIndex = 78;
            this.chkRemove.Text = "Remove dashes";
            this.chkRemove.UseVisualStyleBackColor = true;
            this.chkRemove.CheckedChanged += new System.EventHandler(this.chkRemove_CheckedChanged);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPaste.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnPaste.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste.ForeColor = System.Drawing.Color.White;
            this.btnPaste.Location = new System.Drawing.Point(715, 76);
            this.btnPaste.Margin = new System.Windows.Forms.Padding(2);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(81, 34);
            this.btnPaste.TabIndex = 79;
            this.btnPaste.Tag = "themeable";
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = false;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // CompareForm
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(894, 616);
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
            this.Text = "Compare files";
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
        private ColoredRadioButton chkSHA512;
        private ColoredRadioButton chkSHA384;
        private ColoredRadioButton chkRIPEMD160;
        private ColoredRadioButton chkSHA256;
        private ColoredRadioButton chkSHA1;
        private ColoredRadioButton chkMD5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExpected;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox resultBox;
        private System.Windows.Forms.Panel panel2;
        private ColoredCheckBox chkRemove;
        private ColoredRadioButton chkCRC32;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Label lblNoMatch;
    }
}