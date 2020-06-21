using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_Cloud_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void SetLog(string log)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetLog), new object[] { log });
                return;
            }

            logBox.Text = logBox.Text + Environment.NewLine + log;
            logBox.SelectionStart = logBox.TextLength;
            logBox.ScrollToCaret();
        }

        private void nameHeader_Click(object sender, EventArgs e)
        {
            Console.WriteLine("KLIK!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
