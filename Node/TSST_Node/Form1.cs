using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_Node
{
    public partial class nodeForm : Form
    {
        public string msg;

        public nodeForm()
        {
            InitializeComponent();
            string[] columns = new string[] { "in-port", "szczeliny", "out-port" };
            fibGrid.ColumnCount = columns.Length;

            for (int i = 0; i < columns.Length; i++)
            {
                fibGrid.Columns[i].Name = columns[i];
            }
        }

        public void SetLog(string log)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetLog), new object[] { log });
                return;
            }

            logBox.Text = logBox.Text + Environment.NewLine + log;
        }

        public void SetLabel(string l)
        {
            nameHeader.Text = l;
            this.Text = "Node " + l.Substring(9);
        }

        private void nameHeader_Click(object sender, EventArgs e)
        {
            Console.WriteLine("KLIK!");
        }

        public void AddGrid(FibRow fibRow)
        {
            string[] newRow = new string[] { fibRow.PortFrom, fibRow.First + "-" + fibRow.Last, fibRow.PortTo };

            fibGrid.Rows.Add(newRow);
        }

        public void RemoveGrid(FibRow fibRow)
        {
            string[] newRow = new string[] { fibRow.PortFrom, fibRow.First + "-" + fibRow.Last, fibRow.PortTo };

            for(int i = 0; i < fibGrid.Rows.Count; i++)
            {
                if(fibGrid.Rows[i].Cells[0].Value.ToString() == fibRow.PortFrom && fibGrid.Rows[i].Cells[1].Value.ToString() == fibRow.First + "-" + fibRow.Last && fibGrid.Rows[i].Cells[2].Value.ToString() == fibRow.PortTo)
                    fibGrid.Rows.Remove(fibGrid.Rows[i]);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fibButton_Click(object sender, EventArgs e)
        {

        }
    }
}
