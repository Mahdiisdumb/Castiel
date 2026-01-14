using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Castiel
{
    [ComVisible(true)]
    public class SDKBridge
    {
        public void CreateFile(string path, string content)
        {
            if (EditorForm.GameDir == null) return;
            var full = Path.Combine(EditorForm.GameDir, path);
            Directory.CreateDirectory(Path.GetDirectoryName(full)!);
            File.WriteAllText(full, content);
        }

        public string ReadFile(string path)
        {
            if (EditorForm.GameDir == null) return "";
            var full = Path.Combine(EditorForm.GameDir, path);
            return File.Exists(full) ? File.ReadAllText(full) : "";
        }

        public void CastielSpeak(string msg)
        {
            MessageBox.Show(msg, "Castiel");
        }
    }
}