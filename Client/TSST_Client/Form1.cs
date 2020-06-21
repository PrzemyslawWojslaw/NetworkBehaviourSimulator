using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_Client
{
    public partial class Form1 : Form
    {
        public string msg;
        public string request;

        public Form1()
        {
            InitializeComponent();
            msg = "";
            request = "";
            comboBits.Items.Add("25 Gbit/s");
            comboBits.Items.Add("50 Gbit/s");
            comboBits.Items.Add("75 Gbit/s");
            comboBits.Items.Add("100 Gbit/s");
            comboBits.Items.Add("125 Gbit/s");
            comboBits.Items.Add("150 Gbit/s");
            comboBods.Items.Add("12,5 GBoda");
            comboBods.Items.Add("25 GBodów");
            comboBods.Items.Add("37,5 GBoda");
            comboBods.Items.Add("50 GBodów");
        }

        public void SetLog(string log)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetLog), new object[] { log });
                return;
            }
            string temp = log;
            if (temp.Substring(0, 4) == "2018")
                temp = temp.Substring(13);
            logBox.Text = logBox.Text + Environment.NewLine + temp;
        }

        public void SetLabel(string l)
        {
            nameHeader.Text = l;
            this.Text = "Client " + l.Substring(10);
        }

        public void AddTarget (string name, string first, string last)
        {
            comboTarget.Items.Add(name + " (" + first + "-" + last + ")");
        }

        public void RemoveTarget(string name, string first, string last)
        {
            comboTarget.Items.Remove(name + " (" + first + "-" + last + ")");
        }

        private void nameHeader_Click(object sender, EventArgs e)
        {
            Console.WriteLine("KLIK!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboTarget.Text != "")
                msg = comboTarget.Text + "|" + textMsg.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textRequest.Text != "")
                request = textRequest.Text + "|" + comboBits.Text + "|" + comboBods.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            request = "x|" + comboTarget.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        
    }
}
