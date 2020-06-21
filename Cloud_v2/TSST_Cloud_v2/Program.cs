using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_Cloud_v2
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

            string temp = "cloud_config.txt";

            if (args.Length > 0)
                temp = args[0];

            Cloud cloud = new Cloud(ref window, temp);
            cloud.Start();

            Application.Run(window);

            cloud.Stop();
        }
    }
}
