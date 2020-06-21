namespace TSST_Node
{
    partial class nodeForm
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
            this.logBox = new System.Windows.Forms.TextBox();
            this.nameHeader = new System.Windows.Forms.Label();
            this.fibGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.fibGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(62, 77);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(524, 371);
            this.logBox.TabIndex = 0;
            // 
            // nameHeader
            // 
            this.nameHeader.AutoSize = true;
            this.nameHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nameHeader.Location = new System.Drawing.Point(56, 29);
            this.nameHeader.Name = "nameHeader";
            this.nameHeader.Size = new System.Drawing.Size(130, 31);
            this.nameHeader.TabIndex = 1;
            this.nameHeader.Text = "Klient nr ";
            this.nameHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.nameHeader.Click += new System.EventHandler(this.nameHeader_Click);
            // 
            // fibGrid
            // 
            this.fibGrid.AllowUserToAddRows = false;
            this.fibGrid.AllowUserToDeleteRows = false;
            this.fibGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fibGrid.Location = new System.Drawing.Point(629, 77);
            this.fibGrid.Name = "fibGrid";
            this.fibGrid.RowTemplate.Height = 24;
            this.fibGrid.Size = new System.Drawing.Size(488, 371);
            this.fibGrid.TabIndex = 2;
            // 
            // nodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 474);
            this.Controls.Add(this.fibGrid);
            this.Controls.Add(this.nameHeader);
            this.Controls.Add(this.logBox);
            this.Name = "nodeForm";
            this.Text = "Node - Log viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fibGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label nameHeader;
        private System.Windows.Forms.DataGridView fibGrid;
    }
}

