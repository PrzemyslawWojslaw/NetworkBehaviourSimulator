namespace TSST_NMS
{
    partial class Form1
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

            // początek mojego dodatku
            System.DateTime now = System.DateTime.Now;
            string temp = now.ToString("yyyy.MM.dd-HH.mm");
            temp += " - Dziennik logów [" + nameLabel.Text + "].txt";
            logAll += now.ToString("yyyy.MM.dd - HH:mm.ss.ffff") + "  >>> Wyłączono nasłuchiwanie." + System.Environment.NewLine;
            logAll += now.ToString("yyyy.MM.dd - HH:mm.ss.ffff") + "  >>> Wyłączono NMS." + System.Environment.NewLine;
            System.IO.File.WriteAllText(temp, logAll);
            // koniec mojego dodatku

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nameHeader = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuElements = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveLog = new System.Windows.Forms.ToolStripMenuItem();
            this.pokażDebuglogiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonMap = new System.Windows.Forms.Button();
            this.officialLogs = new System.Windows.Forms.RichTextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameHeader
            // 
            this.nameHeader.AutoSize = true;
            this.nameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameHeader.Location = new System.Drawing.Point(22, 32);
            this.nameHeader.Name = "nameHeader";
            this.nameHeader.Size = new System.Drawing.Size(381, 31);
            this.nameHeader.TabIndex = 1;
            this.nameHeader.Text = "Węzeł sterowania - domena ";
            this.nameHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.nameHeader.Click += new System.EventHandler(this.nameHeader_Click);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(28, 83);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(1080, 403);
            this.logBox.TabIndex = 2;
            this.logBox.Text = "";
            this.logBox.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuElements});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1137, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuElements
            // 
            this.menuElements.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveLog,
            this.pokażDebuglogiToolStripMenuItem});
            this.menuElements.Name = "menuElements";
            this.menuElements.Size = new System.Drawing.Size(66, 24);
            this.menuElements.Text = "Debug";
            // 
            // menuSaveLog
            // 
            this.menuSaveLog.Name = "menuSaveLog";
            this.menuSaveLog.Size = new System.Drawing.Size(236, 26);
            this.menuSaveLog.Text = "Zapisz logi do pliku txt";
            this.menuSaveLog.Click += new System.EventHandler(this.menuSaveLog_Click);
            // 
            // pokażDebuglogiToolStripMenuItem
            // 
            this.pokażDebuglogiToolStripMenuItem.Name = "pokażDebuglogiToolStripMenuItem";
            this.pokażDebuglogiToolStripMenuItem.Size = new System.Drawing.Size(236, 26);
            this.pokażDebuglogiToolStripMenuItem.Text = "Debug-logi";
            this.pokażDebuglogiToolStripMenuItem.Click += new System.EventHandler(this.pokażDebuglogiToolStripMenuItem_Click);
            // 
            // buttonMap
            // 
            this.buttonMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonMap.Location = new System.Drawing.Point(44, 502);
            this.buttonMap.Name = "buttonMap";
            this.buttonMap.Size = new System.Drawing.Size(167, 42);
            this.buttonMap.TabIndex = 4;
            this.buttonMap.Text = "Pokaż dane sieci";
            this.buttonMap.UseVisualStyleBackColor = true;
            this.buttonMap.Click += new System.EventHandler(this.buttonMap_Click);
            // 
            // officialLogs
            // 
            this.officialLogs.Location = new System.Drawing.Point(28, 83);
            this.officialLogs.Name = "officialLogs";
            this.officialLogs.ReadOnly = true;
            this.officialLogs.Size = new System.Drawing.Size(1096, 403);
            this.officialLogs.TabIndex = 5;
            this.officialLogs.Text = "";
            this.officialLogs.WordWrap = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameLabel.Location = new System.Drawing.Point(427, 32);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(81, 31);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "[test]";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 548);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.officialLogs);
            this.Controls.Add(this.buttonMap);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.nameHeader);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Sterowanie - Log viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label nameHeader;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuElements;
        private System.Windows.Forms.ToolStripMenuItem menuSaveLog;
        private System.Windows.Forms.Button buttonMap;
        private System.Windows.Forms.RichTextBox officialLogs;
        private System.Windows.Forms.ToolStripMenuItem pokażDebuglogiToolStripMenuItem;
        private System.Windows.Forms.Label nameLabel;
    }
}

