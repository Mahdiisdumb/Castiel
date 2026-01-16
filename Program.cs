using System;
using System.Threading;
using System.Windows.Forms;

namespace Castiel
{
    static class Program
    {
        private static Mutex? _mutex;

        [STAThread]
        static void Main()
        {
            // Single instance guard
            _mutex = new Mutex(true, "CastielSDK", out bool isNew);
            if (!isNew)
            {
                MessageBox.Show("Castiel is already running.", "Castiel");
                return;
            }

            ApplicationConfiguration.Initialize();

            // Global exception handling
            Application.ThreadException += (s, e) =>
                ShowFatal(e.Exception);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                ShowFatal(e.ExceptionObject as Exception);

            Config.Load();

            Application.ApplicationExit += (_, __) =>
                Config.Save();

            Application.Run(new MainForm());

            _mutex.ReleaseMutex();
        }

        private static void ShowFatal(Exception? ex)
        {
            MessageBox.Show(
                ex?.ToString() ?? "Unknown fatal error.",
                "Castiel Fatal Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}