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
    public partial class NetworkForm : Form
    {
        public bool isVisible;
        public bool needUpdate;
        Dictionary<string, List<string>> fibText = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> cableText = new Dictionary<string, List<string>>();

        public NetworkForm()
        {
            InitializeComponent();
            isVisible = false;
        }

        public void AddElement(string s)
        {
            elementList.Items.Add(s);
        }

        public void RemoveElement(string s)
        {
            elementList.Items.Remove(s);
            if (fibText.ContainsKey(s))
                fibText.Remove(s);
            if (cableText.ContainsKey(s))
                cableText.Remove(s);
        }

        public void EditElementFib(string s, List<string> f)
        {
            if(fibText.ContainsKey(s))
                fibText[s] = f;
            else
                fibText.Add(s, f);
        }

        public void EditElementCable(string s, List<string> c)
        {
            if(cableText.ContainsKey(s))
                cableText[s] = c;
            else
                cableText.Add(s, c);
        }

        private void NetworkForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                isVisible = false;
            }
        }

        private void elementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            chosenElement.Text = elementList.GetItemText(elementList.SelectedItem);
            ChangeGrids(chosenElement.Text);
        }

        private void routingGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void ChangeGrids(string s)
        {
            routingGrid.Columns.Clear();
            cableGrid.Columns.Clear();

            List<string> fib = new List<string>(fibText[s]);
            List<string> cable = new List<string>(cableText[s]);

            if(fibText[s][0] == "node")
                routingGrid.ColumnCount = 3;
            else if (fibText[s][0] == "client")
                routingGrid.ColumnCount = 2;

            fib.RemoveAt(0);

            for(int i = 0; i < routingGrid.ColumnCount; i++)
            {
                routingGrid.Columns[i].Name = fib[i];
            }

            fib.RemoveRange(0, routingGrid.ColumnCount);

            string[] newRow1 = new string[routingGrid.ColumnCount];
            for (int i = 0; i < fib.Count - routingGrid.ColumnCount + 1; i += routingGrid.ColumnCount)
            {
                newRow1[0] = fib[i];
                newRow1[1] = fib[i + 1];
                if(routingGrid.ColumnCount == 3)
                    newRow1[2] = fib[i + 2];
                routingGrid.Rows.Add(newRow1);
            }


            cableGrid.ColumnCount = 5;

            for (int i = 0; i < cableGrid.ColumnCount; i++)
            {
                cableGrid.Columns[i].Name = cable[i];
            }

            cable.RemoveRange(0, cableGrid.ColumnCount);

            string[] newRow2 = new string[cableGrid.ColumnCount];
            for (int i = 0; i < cable.Count - cableGrid.ColumnCount + 1; i += cableGrid.ColumnCount)
            {
                newRow2[0] = cable[i];
                newRow2[1] = cable[i + 1];
                newRow2[2] = cable[i + 2];
                newRow2[3] = cable[i + 3];
                newRow2[4] = cable[i + 4];
                cableGrid.Rows.Add(newRow2);
            }
        }
    }
}
