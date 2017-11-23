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
            this.chkCRC32 = new System.Windows.Forms.RadioButton();
            this.chkSHA512 = new System.Windows.Forms.RadioButton();
            this.chkSHA384 = new System.Windows.Forms.RadioButton();
            this.chkRIPEMD160 = new System.Windows.Forms.RadioButton();
            this.chkSHA256 = new System.Windows.Forms.RadioButton();
            this.chkSHA1 = new System.Windows.Forms.RadioButton();
            this.chkMD5 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExpected = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.resultBox = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblNoMatch = new System.Windows.Forms.Label();
            this.chkRemove = new System.Windows.Forms.CheckBox();
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
            this.panel1.Size = new System.Drawing.Size(715, 32);
            this.panel1.TabIndex = 4;
            // 
            // chkCRC32
            // 
            this.chkCRC32.AutoSize = true;
            this.chkCRC32.Location = new System.Drawing.Point(333, 7);
            this.chkCRC32.Margin = new System.Windows.Forms.Padding(2);
            this.chkCRC32.Name = "chkCRC32";
            this.chkCRC32.Size = new System.Drawing.Size(60, 19);
            this.chkCRC32.TabIndex = 6;
            this.chkCRC32.Text = "CRC32";
            this.chkCRC32.UseVisualStyleBackColor = true;
            // 
            // chkSHA512
            // 
            this.chkSHA512.AutoSize = true;
            this.chkSHA512.Location = new System.Drawing.Point(261, 7);
            this.chkSHA512.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA512.Name = "chkSHA512";
            this.chkSHA512.Size = new System.Drawing.Size(68, 19);
            this.chkSHA512.TabIndex = 5;
            this.chkSHA512.Text = "SHA512";
            this.chkSHA512.UseVisualStyleBackColor = true;
            // 
            // chkSHA384
            // 
            this.chkSHA384.AutoSize = true;
            this.chkSHA384.Location = new System.Drawing.Point(190, 7);
            this.chkSHA384.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA384.Name = "chkSHA384";
            this.chkSHA384.Size = new System.Drawing.Size(70, 19);
            this.chkSHA384.TabIndex = 4;
            this.chkSHA384.Text = "SHA384";
            this.chkSHA384.UseVisualStyleBackColor = true;
            // 
            // chkRIPEMD160
            // 
            this.chkRIPEMD160.AutoSize = true;
            this.chkRIPEMD160.Location = new System.Drawing.Point(397, 7);
            this.chkRIPEMD160.Margin = new System.Windows.Forms.Padding(2);
            this.chkRIPEMD160.Name = "chkRIPEMD160";
            this.chkRIPEMD160.Size = new System.Drawing.Size(88, 19);
            this.chkRIPEMD160.TabIndex = 3;
            this.chkRIPEMD160.Text = "RIPEMD160";
            this.chkRIPEMD160.UseVisualStyleBackColor = true;
            // 
            // chkSHA256
            // 
            this.chkSHA256.AutoSize = true;
            this.chkSHA256.Location = new System.Drawing.Point(121, 7);
            this.chkSHA256.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA256.Name = "chkSHA256";
            this.chkSHA256.Size = new System.Drawing.Size(70, 19);
            this.chkSHA256.TabIndex = 2;
            this.chkSHA256.Text = "SHA256";
            this.chkSHA256.UseVisualStyleBackColor = true;
            // 
            // chkSHA1
            // 
            this.chkSHA1.AutoSize = true;
            this.chkSHA1.Location = new System.Drawing.Point(64, 7);
            this.chkSHA1.Margin = new System.Windows.Forms.Padding(2);
            this.chkSHA1.Name = "chkSHA1";
            this.chkSHA1.Size = new System.Drawing.Size(54, 19);
            this.chkSHA1.TabIndex = 1;
            this.chkSHA1.Text = "SHA1";
            this.chkSHA1.UseVisualStyleBackColor = true;
            // 
            // chkMD5
            // 
            this.chkMD5.AutoSize = true;
            this.chkMD5.Checked = true;
            this.chkMD5.Location = new System.Drawing.Point(9, 7);
            this.chkMD5.Margin = new System.Windows.Forms.Padding(2);
            this.chkMD5.Name = "chkMD5";
            this.chkMD5.Size = new System.Drawing.Size(52, 19);
            this.chkMD5.TabIndex = 0;
            this.chkMD5.TabStop = true;
            this.chkMD5.Text = "MD5";
            this.chkMD5.UseVisualStyleBackColor = true;
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
            this.txtExpected.Size = new System.Drawing.Size(558, 27);
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
            this.btnCompare.Location = new System.Drawing.Point(641, 61);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(65, 27);
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
            // resultBox
            // 
            this.resultBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.resultBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultBox.ForeColor = System.Drawing.Color.White;
            this.resultBox.FormattingEnabled = true;
            this.resultBox.HorizontalScrollbar = true;
            this.resultBox.ItemHeight = 21;
            this.resultBox.Location = new System.Drawing.Point(0, 0);
            this.resultBox.Margin = new System.Windows.Forms.Padding(2);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(694, 347);
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
            this.panel2.Location = new System.Drawing.Point(10, 135);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(696, 349);
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
            this.lblNoMatch.Size = new System.Drawing.Size(694, 347);
            this.lblNoMatch.TabIndex = 77;
            this.lblNoMatch.Tag = "themeable";
            this.lblNoMatch.Text = "No files matching";
            this.lblNoMatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPaste.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnPaste.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste.ForeColor = System.Drawing.Color.White;
            this.btnPaste.Location = new System.Drawing.Point(572, 61);
            this.btnPaste.Margin = new System.Windows.Forms.Padding(2);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(65, 27);
            this.btnPaste.TabIndex = 79;
            this.btnPaste.Tag = "themeable";
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = false;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // CompareForm
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(715, 493);
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
            this.Load += new System.EventHandler(this.CompareForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton chkSHA512;
        private System.Windows.Forms.RadioButton chkSHA384;
        private System.Windows.Forms.RadioButton chkRIPEMD160;
        private System.Windows.Forms.RadioButton chkSHA256;
        private System.Windows.Forms.RadioButton chkSHA1;
        private System.Windows.Forms.RadioButton chkMD5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExpected;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox resultBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkRemove;
        private System.Windows.Forms.RadioButton chkCRC32;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Label lblNoMatch;
    }
}