using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_NMS
{
    public partial class Form1 : Form
    {
        NetworkForm netwindow = new NetworkForm();
        string logAll;

        public Form1()
        {
            InitializeComponent();
        }

        public NetworkForm Netwindow
        {
            get { return netwindow; }
        }

        public string NameLabel
        {
            get { return nameLabel.Text; }
        }

        public void SetLabel(string l)
        {
            nameLabel.Text = l;
            this.Text = "Domain " + nameLabel.Text;
            DateTime now = DateTime.Now;
            logAll = "Dziennik logów domeny \"" + nameLabel.Text + "\" z dnia " + now.ToString("dd.MM.yyyy") + Environment.NewLine + "=====================================" + Environment.NewLine;
        }

        public void SetLog(string log)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetLog), new object[] { log });
                return;
            }

            logBox.Text = logBox.Text + log + Environment.NewLine;
            logBox.SelectionStart = logBox.TextLength;
            logBox.ScrollToCaret();
            logAll = logAll + log + Environment.NewLine;

        }

        public void SetOfficial(string log)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetOfficial), new object[] { log });
                return;
            }

            string temp = log;
            if (temp.Substring(0, 4) == "2018")
            {
                temp = " #" + temp.Substring(13);
            }
                

            officialLogs.Text = officialLogs.Text + temp + Environment.NewLine;
            officialLogs.SelectionStart = officialLogs.TextLength;
            officialLogs.ScrollToCaret();

            HighlightPhrase(officialLogs, " NCC ", Color.Red);
            HighlightPhrase(officialLogs, " P ", Color.Blue);
            HighlightPhrase(officialLogs, " CC ", Color.Green);
            HighlightPhrase(officialLogs, " RC ", Color.Brown);
            HighlightPhrase(officialLogs, " LRM ", Color.Purple);
            HighlightDate(officialLogs, Color.Black);

            SetLog(log);
        }

        public void HighlightPhrase(RichTextBox box, string phrase, Color color)
        {
            int pos = box.SelectionStart;
            string s = box.Text;
            for (int ix = 0; ;)
            {
                int jx = s.IndexOf(phrase, ix, StringComparison.CurrentCultureIgnoreCase);
                if (jx < 0)
                    break;
                box.SelectionStart = jx;
                box.SelectionLength = phrase.Length;
                box.SelectionColor = color;
                box.SelectionFont = new Font(box.Font, FontStyle.Bold);
                ix = jx + 1;
            }
            box.SelectionStart = pos;
            box.SelectionLength = 0;
        }

        public void HighlightDate(RichTextBox box, Color color)
        {
            int pos = box.SelectionStart;
            string s = box.Text;
            for (int ix = 0; ;)
            {
                int jx = s.IndexOf("#", ix, StringComparison.CurrentCultureIgnoreCase); // # to znak wyróżniający
                if (jx < 0)
                    break;
                box.SelectionStart = jx;
                box.SelectionLength = 14;  // długość daty + znaku wyróżniającego
                box.SelectionColor = color;
                box.SelectionFont = new Font(box.Font, FontStyle.Bold);
                ix = jx + 1;
            }
            box.SelectionStart = pos;
            box.SelectionLength = 0;
        }

        private void nameHeader_Click(object sender, EventArgs e)
        {
            Console.WriteLine("KLIK!");
        }

        private void buttonMap_Click(object sender, EventArgs e)
        {
            netwindow.Show();
            netwindow.isVisible = true;
        }

        private void menuSaveLog_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string temp = now.ToString("yyyy.MM.dd");
            temp += " - Dziennik logów.txt";
            System.IO.File.WriteAllText(temp, logAll);
        }

        private void pokażDebuglogiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logBox.Visible = !logBox.Visible;
            officialLogs.Visible = !officialLogs.Visible;
        }
    }
}
