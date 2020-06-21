using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 window = new Form1();
            string tempN;  // nazwa klienta
            string tempD;  // nazwa domeny, do której jest podłączony

            if (args.Length == 0)
            {
                tempN = "1";  // zostanie dodane "c" później
                tempD = "1";
            }
            else
            {
                tempN = args[1];
                tempD = args[0];
            }

            Client client = new Client(ref window, tempD, tempN);
            client.Start();

            Application.Run(window);

            client.Stop();
        }
    }
}
