using Microsoft.Web.WebView2.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Castiel
{
    public class EditorForm : Form
    {
        public static string? GameDir; // nullable to avoid CS8618
        private WebView2 preview;

        public EditorForm(string dir)
        {
            GameDir = dir;

            Width = 1200;
            Height = 700;
            Text = "Castiel Editor";

            var split = new SplitContainer { Dock = DockStyle.Fill };
            Controls.Add(split);

            var tabs = new TabControl { Dock = DockStyle.Fill };
            split.Panel1.Controls.Add(tabs);

            tabs.TabPages.Add(MakeEditorTab("run.html"));
            tabs.TabPages.Add(MakeEditorTab("style.css"));
            tabs.TabPages.Add(MakeEditorTab("game.js"));

            preview = new WebView2 { Dock = DockStyle.Fill };
            split.Panel2.Controls.Add(preview);

            preview.CoreWebView2InitializationCompleted += (s, e) =>
            {
                preview.CoreWebView2.AddHostObjectToScript("CastielHost", new SDKBridge());
                preview.CoreWebView2.Navigate(Path.Combine(GameDir, "run.html"));
            };

            preview.EnsureCoreWebView2Async();
        }

        private TabPage MakeEditorTab(string file)
        {
            var path = Path.Combine(GameDir!, file);
            var box = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Consolas", 10),
                Text = File.Exists(path) ? File.ReadAllText(path) : ""
            };

            box.TextChanged += (s, e) =>
                File.WriteAllText(path, box.Text);

            var tab = new TabPage(file);
            tab.Controls.Add(box);
            return tab;
        }
    }
}