using System;
using System.Windows.Forms;

namespace Heartache
{
    class Program
    {
        private static UI.Heartache heartacheUI = null;

        [STAThread]
        static void Main(string[] args)
        {
            heartacheUI = new UI.Heartache();

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Application.Run(heartacheUI);
        }

        static void OnApplicationExit(object o, EventArgs e)
        {
            HeartacheSettings.Default.Save();
        }
    }
}
