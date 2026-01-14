using System;
using System.Windows.Forms;

namespace Castiel
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Config.Load();
            Application.Run(new MainForm());
        }
    }
}