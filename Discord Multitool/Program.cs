using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Discord_Multitool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>


        [STAThread]
        static void Main()
        {
            ServicePointManager.DefaultConnectionLimit = 100000000;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }

    }
}