using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_NMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) // args = { name (cyfra), config.txt }
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 window = new Form1();
            string name;

            if (args.Length == 0)
                name = "d1";
            else
                name = args[0];


            Nms nms = new Nms(ref window, name);
            nms.Start();

            Application.Run(window);

            nms.Stop();
        }
    }
}
