namespace Hashing
{
    partial class IdenticalsForm
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
            this.SumView = new MoonTree();
            this.helperMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioCRC32 = new Hashing.MoonRadio();
            this.radioSHA512 = new Hashing.MoonRadio();
            this.radioSHA384 = new Hashing.MoonRadio();
            this.radioRIPEMD160 = new Hashing.MoonRadio();
            this.radioSHA256 = new Hashing.MoonRadio();
            this.radioSHA1 = new Hashing.MoonRadio();
            this.radioMD5 = new Hashing.MoonRadio();
            this.helperMenu.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SumView
            // 
            this.SumView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.SumView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SumView.ContextMenuStrip = this.helperMenu;
            this.SumView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SumView.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SumView.ForeColor = System.Drawing.Color.White;
            this.SumView.Location = new System.Drawing.Point(0, 0);
            this.SumView.Margin = new System.Windows.Forms.Padding(2);
            this.SumView.Name = "SumView";
            this.SumView.Size = new System.Drawing.Size(713, 459);
            this.SumView.TabIndex = 1;
            this.SumView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SumView_NodeMouseClick);
            // 
            // helperMenu
            // 
            this.helperMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.helperMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.helperMenu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helperMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.helperMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.saveAsJSONToolStripMenuItem});
            this.helperMenu.Name = "helperMenu";
            this.helperMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.helperMenu.ShowImageMargin = false;
            this.helperMenu.Size = new System.Drawing.Size(156, 74);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(155, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // saveAsJSONToolStripMenuItem
            // 
            this.saveAsJSONToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAsJSONToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAsJSONToolStripMenuItem.Name = "saveAsJSONToolStripMenuItem";
            this.saveAsJSONToolStripMenuItem.Size = new System.Drawing.Size(155, 24);
            this.saveAsJSONToolStripMenuItem.Text = "Save as JSON";
            this.saveAsJSONToolStripMenuItem.Click += new System.EventHandler(this.saveAsJSONToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.SumView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(715, 461);
            this.panel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioCRC32);
            this.panel1.Controls.Add(this.radioSHA512);
            this.panel1.Controls.Add(this.radioSHA384);
            this.panel1.Controls.Add(this.radioRIPEMD160);
            this.panel1.Controls.Add(this.radioSHA256);
            this.panel1.Controls.Add(this.radioSHA1);
            this.panel1.Controls.Add(this.radioMD5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 32);
            this.panel1.TabIndex = 3;
            // 
            // radioCRC32
            // 
            this.radioCRC32.AutoSize = true;
            this.radioCRC32.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioCRC32.Location = new System.Drawing.Point(380, 4);
            this.radioCRC32.Margin = new System.Windows.Forms.Padding(2);
            this.radioCRC32.Name = "radioCRC32";
            this.radioCRC32.Size = new System.Drawing.Size(70, 23);
            this.radioCRC32.TabIndex = 6;
            this.radioCRC32.TabStop = true;
            this.radioCRC32.Text = "CRC32";
            this.radioCRC32.UseVisualStyleBackColor = true;
            this.radioCRC32.CheckedChanged += new System.EventHandler(this.radioCRC32_CheckedChanged);
            // 
            // radioSHA512
            // 
            this.radioSHA512.AutoSize = true;
            this.radioSHA512.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioSHA512.Location = new System.Drawing.Point(300, 4);
            this.radioSHA512.Margin = new System.Windows.Forms.Padding(2);
            this.radioSHA512.Name = "radioSHA512";
            this.radioSHA512.Size = new System.Drawing.Size(76, 23);
            this.radioSHA512.TabIndex = 5;
            this.radioSHA512.TabStop = true;
            this.radioSHA512.Text = "SHA512";
            this.radioSHA512.UseVisualStyleBackColor = true;
            this.radioSHA512.CheckedChanged += new System.EventHandler(this.radioSHA512_CheckedChanged);
            // 
            // radioSHA384
            // 
            this.radioSHA384.AutoSize = true;
            this.radioSHA384.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioSHA384.Location = new System.Drawing.Point(218, 4);
            this.radioSHA384.Margin = new System.Windows.Forms.Padding(2);
            this.radioSHA384.Name = "radioSHA384";
            this.radioSHA384.Size = new System.Drawing.Size(78, 23);
            this.radioSHA384.TabIndex = 4;
            this.radioSHA384.TabStop = true;
            this.radioSHA384.Text = "SHA384";
            this.radioSHA384.UseVisualStyleBackColor = true;
            this.radioSHA384.CheckedChanged += new System.EventHandler(this.radioSHA384_CheckedChanged);
            // 
            // radioRIPEMD160
            // 
            this.radioRIPEMD160.AutoSize = true;
            this.radioRIPEMD160.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioRIPEMD160.Location = new System.Drawing.Point(454, 4);
            this.radioRIPEMD160.Margin = new System.Windows.Forms.Padding(2);
            this.radioRIPEMD160.Name = "radioRIPEMD160";
            this.radioRIPEMD160.Size = new System.Drawing.Size(100, 23);
            this.radioRIPEMD160.TabIndex = 3;
            this.radioRIPEMD160.TabStop = true;
            this.radioRIPEMD160.Text = "RIPEMD160";
            this.radioRIPEMD160.UseVisualStyleBackColor = true;
            this.radioRIPEMD160.CheckedChanged += new System.EventHandler(this.radioRIPEMD160_CheckedChanged);
            // 
            // radioSHA256
            // 
            this.radioSHA256.AutoSize = true;
            this.radioSHA256.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioSHA256.Location = new System.Drawing.Point(136, 4);
            this.radioSHA256.Margin = new System.Windows.Forms.Padding(2);
            this.radioSHA256.Name = "radioSHA256";
            this.radioSHA256.Size = new System.Drawing.Size(78, 23);
            this.radioSHA256.TabIndex = 2;
            this.radioSHA256.TabStop = true;
            this.radioSHA256.Text = "SHA256";
            this.radioSHA256.UseVisualStyleBackColor = true;
            this.radioSHA256.CheckedChanged += new System.EventHandler(this.radioSHA256_CheckedChanged);
            // 
            // radioSHA1
            // 
            this.radioSHA1.AutoSize = true;
            this.radioSHA1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioSHA1.Location = new System.Drawing.Point(72, 4);
            this.radioSHA1.Margin = new System.Windows.Forms.Padding(2);
            this.radioSHA1.Name = "radioSHA1";
            this.radioSHA1.Size = new System.Drawing.Size(60, 23);
            this.radioSHA1.TabIndex = 1;
            this.radioSHA1.TabStop = true;
            this.radioSHA1.Text = "SHA1";
            this.radioSHA1.UseVisualStyleBackColor = true;
            this.radioSHA1.CheckedChanged += new System.EventHandler(this.radioSHA1_CheckedChanged);
            // 
            // radioMD5
            // 
            this.radioMD5.AutoSize = true;
            this.radioMD5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioMD5.Location = new System.Drawing.Point(10, 4);
            this.radioMD5.Margin = new System.Windows.Forms.Padding(2);
            this.radioMD5.Name = "radioMD5";
            this.radioMD5.Size = new System.Drawing.Size(58, 23);
            this.radioMD5.TabIndex = 0;
            this.radioMD5.TabStop = true;
            this.radioMD5.Text = "MD5";
            this.radioMD5.UseVisualStyleBackColor = true;
            this.radioMD5.CheckedChanged += new System.EventHandler(this.radioMD5_CheckedChanged);
            // 
            // IdenticalsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(715, 493);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.Name = "IdenticalsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Identical files";
            this.Load += new System.EventHandler(this.CompareForm_Load);
            this.helperMenu.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MoonTree SumView;
        private System.Windows.Forms.ContextMenuStrip helperMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsJSONToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private MoonRadio radioMD5;
        private MoonRadio radioSHA1;
        private MoonRadio radioSHA256;
        private MoonRadio radioRIPEMD160;
        private MoonRadio radioSHA384;
        private MoonRadio radioSHA512;
        private System.Windows.Forms.Panel panel1;
        private MoonRadio radioCRC32;
    }
}