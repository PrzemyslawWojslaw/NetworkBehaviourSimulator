using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSST_Node
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

            nodeForm window = new nodeForm();

            Node node = new Node(ref window, args[0], args[1]);
            node.Start();

            Application.Run(window);

            node.Stop();
        }
    }
}
