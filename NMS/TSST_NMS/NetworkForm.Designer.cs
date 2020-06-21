namespace TSST_NMS
{
    partial class NetworkForm
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
            this.elementList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chosenElement = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.routingGrid = new System.Windows.Forms.DataGridView();
            this.cableGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.routingGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // elementList
            // 
            this.elementList.FormattingEnabled = true;
            this.elementList.ItemHeight = 16;
            this.elementList.Location = new System.Drawing.Point(22, 66);
            this.elementList.Name = "elementList";
            this.elementList.Size = new System.Drawing.Size(213, 548);
            this.elementList.TabIndex = 0;
            this.elementList.SelectedIndexChanged += new System.EventHandler(this.elementList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lista elementów w domenie";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(283, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Wybrany element:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Zawartość tablicy routingu:";
            // 
            // chosenElement
            // 
            this.chosenElement.AutoSize = true;
            this.chosenElement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chosenElement.Location = new System.Drawing.Point(464, 66);
            this.chosenElement.Name = "chosenElement";
            this.chosenElement.Size = new System.Drawing.Size(67, 20);
            this.chosenElement.TabIndex = 5;
            this.chosenElement.Text = "<brak>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(283, 375);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Połączenia fizyczne (kable):";
            // 
            // routingGrid
            // 
            this.routingGrid.AllowUserToAddRows = false;
            this.routingGrid.AllowUserToDeleteRows = false;
            this.routingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.routingGrid.Location = new System.Drawing.Point(286, 132);
            this.routingGrid.Name = "routingGrid";
            this.routingGrid.RowTemplate.Height = 24;
            this.routingGrid.Size = new System.Drawing.Size(460, 219);
            this.routingGrid.TabIndex = 9;
            // 
            // cableGrid
            // 
            this.cableGrid.AllowUserToAddRows = false;
            this.cableGrid.AllowUserToDeleteRows = false;
            this.cableGrid.AllowUserToResizeColumns = false;
            this.cableGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cableGrid.Location = new System.Drawing.Point(286, 414);
            this.cableGrid.Name = "cableGrid";
            this.cableGrid.RowTemplate.Height = 24;
            this.cableGrid.Size = new System.Drawing.Size(727, 200);
            this.cableGrid.TabIndex = 10;
            // 
            // NetworkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 639);
            this.Controls.Add(this.cableGrid);
            this.Controls.Add(this.routingGrid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chosenElement);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.elementList);
            this.Name = "NetworkForm";
            this.Text = "NMS - elementy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.routingGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cableGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox elementList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label chosenElement;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridView routingGrid;
        private System.Windows.Forms.DataGridView cableGrid;
    }
}