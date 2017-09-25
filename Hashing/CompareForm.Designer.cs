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
            this.chkRemove = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.panel1.Size = new System.Drawing.Size(641, 40);
            this.panel1.TabIndex = 4;
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
            // 
            // chkRIPEMD160
            // 
            this.chkRIPEMD160.AutoSize = true;
            this.chkRIPEMD160.Location = new System.Drawing.Point(411, 9);
            this.chkRIPEMD160.Margin = new System.Windows.Forms.Padding(2);
            this.chkRIPEMD160.Name = "chkRIPEMD160";
            this.chkRIPEMD160.Size = new System.Drawing.Size(107, 24);
            this.chkRIPEMD160.TabIndex = 3;
            this.chkRIPEMD160.Text = "RIPEMD160";
            this.chkRIPEMD160.UseVisualStyleBackColor = true;
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
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(7, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 28);
            this.label1.TabIndex = 5;
            this.label1.Tag = "themeable";
            this.label1.Text = "Expected hash:";
            // 
            // txtExpected
            // 
            this.txtExpected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.txtExpected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExpected.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtExpected.ForeColor = System.Drawing.Color.White;
            this.txtExpected.Location = new System.Drawing.Point(12, 78);
            this.txtExpected.Name = "txtExpected";
            this.txtExpected.Size = new System.Drawing.Size(500, 27);
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
            this.btnCompare.Location = new System.Drawing.Point(520, 73);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(110, 35);
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
            this.label2.Location = new System.Drawing.Point(7, 138);
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
            this.resultBox.ForeColor = System.Drawing.Color.White;
            this.resultBox.FormattingEnabled = true;
            this.resultBox.ItemHeight = 20;
            this.resultBox.Location = new System.Drawing.Point(0, 0);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(615, 317);
            this.resultBox.TabIndex = 76;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.resultBox);
            this.panel2.Location = new System.Drawing.Point(12, 169);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(617, 319);
            this.panel2.TabIndex = 77;
            // 
            // chkRemove
            // 
            this.chkRemove.AutoSize = true;
            this.chkRemove.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemove.Location = new System.Drawing.Point(12, 111);
            this.chkRemove.Name = "chkRemove";
            this.chkRemove.Size = new System.Drawing.Size(136, 24);
            this.chkRemove.TabIndex = 78;
            this.chkRemove.Text = "Remove dashes";
            this.chkRemove.UseVisualStyleBackColor = true;
            this.chkRemove.CheckedChanged += new System.EventHandler(this.chkRemove_CheckedChanged);
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(641, 500);
            this.Controls.Add(this.chkRemove);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.txtExpected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompareForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
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
    }
}