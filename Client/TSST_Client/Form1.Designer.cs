namespace TSST_Client
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
            this.button1 = new System.Windows.Forms.Button();
            this.textRequest = new System.Windows.Forms.TextBox();
            this.textMsg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboTarget = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBits = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBods = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(62, 77);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(524, 346);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(686, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 38);
            this.button1.TabIndex = 2;
            this.button1.Text = "Wyślij wiadomość";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textRequest
            // 
            this.textRequest.Location = new System.Drawing.Point(607, 108);
            this.textRequest.Name = "textRequest";
            this.textRequest.Size = new System.Drawing.Size(161, 22);
            this.textRequest.TabIndex = 3;
            // 
            // textMsg
            // 
            this.textMsg.Location = new System.Drawing.Point(604, 301);
            this.textMsg.Name = "textMsg";
            this.textMsg.Size = new System.Drawing.Size(296, 22);
            this.textMsg.TabIndex = 4;
            this.textMsg.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(601, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Do kogo:";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(601, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Treść wiadomości:";
            // 
            // comboTarget
            // 
            this.comboTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTarget.FormattingEnabled = true;
            this.comboTarget.Location = new System.Drawing.Point(604, 231);
            this.comboTarget.Name = "comboTarget";
            this.comboTarget.Size = new System.Drawing.Size(296, 24);
            this.comboTarget.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(604, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Zestaw połącznie:";
            // 
            // comboBits
            // 
            this.comboBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBits.FormattingEnabled = true;
            this.comboBits.Location = new System.Drawing.Point(774, 108);
            this.comboBits.Name = "comboBits";
            this.comboBits.Size = new System.Drawing.Size(121, 24);
            this.comboBits.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(686, 140);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Wyślij żądanie";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(686, 392);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(154, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Zerwij połączenie";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBods
            // 
            this.comboBods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBods.FormattingEnabled = true;
            this.comboBods.Location = new System.Drawing.Point(774, 77);
            this.comboBods.Name = "comboBods";
            this.comboBods.Size = new System.Drawing.Size(121, 24);
            this.comboBods.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 435);
            this.Controls.Add(this.comboBods);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBits);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboTarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textMsg);
            this.Controls.Add(this.textRequest);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nameHeader);
            this.Controls.Add(this.logBox);
            this.Name = "Form1";
            this.Text = "Client - Log viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label nameHeader;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textRequest;
        private System.Windows.Forms.TextBox textMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboTarget;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBits;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBods;
    }
}

