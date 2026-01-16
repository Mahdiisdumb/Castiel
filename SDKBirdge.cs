using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Castiel
{
    [ComVisible(true)]
    public class SDKBridge
    {
        private static string? Root => EditorForm.GameDir;

        private static string ResolvePath(string relativePath)
        {
            if (Root == null)
                throw new InvalidOperationException("Game directory not set.");

            if (relativePath.Contains(".."))
                throw new InvalidOperationException("Path traversal detected.");

            string full = Path.GetFullPath(Path.Combine(Root, relativePath));

            if (!full.StartsWith(Path.GetFullPath(Root)))
                throw new InvalidOperationException("Invalid path.");

            return full;
        }

        public bool CreateFile(string path, string content)
        {
            try
            {
                if (content.Length > 2_000_000) // 2MB cap
                    throw new Exception("File too large.");

                string full = ResolvePath(path);

                Directory.CreateDirectory(Path.GetDirectoryName(full)!);
                File.WriteAllText(full, content);
                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                return false;
            }
        }

        public string ReadFile(string path)
        {
            try
            {
                string full = ResolvePath(path);
                return File.Exists(full) ? File.ReadAllText(full) : "";
            }
            catch
            {
                return "";
            }
        }

        private DateTime _lastPopup = DateTime.MinValue;

        public void CastielSpeak(string msg)
        {
            // rate-limit message spam
            if ((DateTime.Now - _lastPopup).TotalSeconds < 1)
                return;

            _lastPopup = DateTime.Now;
            MessageBox.Show(msg, "Castiel");
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(
                msg,
                "Castiel SDK Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}
