using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Castiel
{
    public class EditorForm : Form
    {
        public static string? GameDir;
        private WebView2 preview;
        private TabControl tabs;
        private TreeView projectTree;
        private ToolStripStatusLabel statusLabel;

        public EditorForm(string dir)
        {
            GameDir = dir;

            Text = $"Castiel Editor — {Path.GetFileName(dir)}";
            Width = 1400;
            Height = 800;
            StartPosition = FormStartPosition.CenterScreen;

            // ===== MENU =====
            var menu = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Save All", null, (_, __) => SaveAll());
            fileMenu.DropDownItems.Add("Open Folder", null, (_, __) => OpenFolder());
            fileMenu.DropDownItems.Add("Close", null, (_, __) => Close());

            var runMenu = new ToolStripMenuItem("Run");
            runMenu.DropDownItems.Add("Refresh Preview", null, (_, __) => ReloadPreview());

            menu.Items.Add(fileMenu);
            menu.Items.Add(runMenu);
            MainMenuStrip = menu;
            Controls.Add(menu);

            // ===== TOOLBAR =====
            var toolbar = new ToolStrip();
            toolbar.Items.Add(new ToolStripButton("Save", null, (_, __) => SaveAll()));
            toolbar.Items.Add(new ToolStripButton("Refresh", null, (_, __) => ReloadPreview()));
            toolbar.Dock = DockStyle.Top;
            Controls.Add(toolbar);

            // ===== STATUS BAR =====
            var status = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready");
            status.Items.Add(statusLabel);
            Controls.Add(status);

            // ===== MAIN SPLIT =====
            var mainSplit = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 250 };
            Controls.Add(mainSplit);
            mainSplit.BringToFront();

            // ===== PROJECT TREE =====
            projectTree = new TreeView { Dock = DockStyle.Fill };
            projectTree.NodeMouseDoubleClick += (_, e) =>
            {
                if (File.Exists(e.Node.Tag?.ToString()))
                    OpenFileTab(e.Node.Tag!.ToString()!);
            };
            mainSplit.Panel1.Controls.Add(projectTree);
            LoadProjectTree();

            // ===== EDITOR / PREVIEW SPLIT =====
            var editorSplit = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 650 };
            mainSplit.Panel2.Controls.Add(editorSplit);

            tabs = new TabControl { Dock = DockStyle.Fill };
            editorSplit.Panel1.Controls.Add(tabs);

            preview = new WebView2 { Dock = DockStyle.Fill };
            editorSplit.Panel2.Controls.Add(preview);

            preview.CoreWebView2InitializationCompleted += (_, __) =>
            {
                preview.CoreWebView2.AddHostObjectToScript("CastielHost", new SDKBridge());
                ReloadPreview();
            };

            preview.EnsureCoreWebView2Async();
        }

        private void LoadProjectTree()
        {
            projectTree.Nodes.Clear();
            var root = new TreeNode(Path.GetFileName(GameDir!));
            foreach (var file in Directory.GetFiles(GameDir!))
                root.Nodes.Add(new TreeNode(Path.GetFileName(file)) { Tag = file });

            projectTree.Nodes.Add(root);
            root.Expand();
        }

        private void OpenFileTab(string path)
        {
            foreach (TabPage t in tabs.TabPages)
                if (t.Tag?.ToString() == path) { tabs.SelectedTab = t; return; }

            var box = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 10),
                AcceptsTab = true,
                WordWrap = false,
                Text = File.ReadAllText(path)
            };

            Timer saveTimer = new Timer(500) { AutoReset = false };
            saveTimer.Elapsed += (_, __) =>
            {
                File.WriteAllText(path, box.Text);
                statusLabel.Text = $"Saved {Path.GetFileName(path)}";
            };

            box.TextChanged += (_, __) =>
            {
                saveTimer.Stop();
                saveTimer.Start();
            };

            var tab = new TabPage(Path.GetFileName(path)) { Tag = path };
            tab.Controls.Add(box);

            tabs.TabPages.Add(tab);
            tabs.SelectedTab = tab;
        }

        private void SaveAll()
        {
            foreach (TabPage t in tabs.TabPages)
                if (t.Controls[0] is TextBox box && t.Tag is string path)
                    File.WriteAllText(path, box.Text);

            statusLabel.Text = "All files saved";
        }

        private void ReloadPreview()
        {
            if (preview.CoreWebView2 == null) return;
            string page = Path.Combine(GameDir!, "run.html");
            preview.CoreWebView2.Navigate(page);
            statusLabel.Text = "Preview refreshed";
        }

        private void OpenFolder()
        {
            System.Diagnostics.Process.Start("explorer", GameDir!);
        }
    }
}