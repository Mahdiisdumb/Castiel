using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
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
            BackColor = Color.FromArgb(30, 30, 30); // Dark background

            // ===== MENU =====
            var menu = new MenuStrip { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Save All", null, (_, __) => SaveAll());
            fileMenu.DropDownItems.Add("Open Folder", null, (_, __) => OpenFolder());
            fileMenu.DropDownItems.Add("Import Asset", null, (_, __) => ImportAsset());
            fileMenu.DropDownItems.Add("Close", null, (_, __) => Close());

            var runMenu = new ToolStripMenuItem("Run");
            runMenu.DropDownItems.Add("Refresh Preview", null, (_, __) => ReloadPreview());

            menu.Items.Add(fileMenu);
            menu.Items.Add(runMenu);
            MainMenuStrip = menu;
            Controls.Add(menu);

            // ===== TOOLBAR =====
            var toolbar = new ToolStrip { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };
            toolbar.Items.Add(new ToolStripButton("Save", null, (_, __) => SaveAll()));
            toolbar.Items.Add(new ToolStripButton("Refresh", null, (_, __) => ReloadPreview()));
            toolbar.Items.Add(new ToolStripButton("Import Asset", null, (_, __) => ImportAsset()));
            toolbar.Dock = DockStyle.Top;
            Controls.Add(toolbar);

            // ===== STATUS BAR =====
            var status = new StatusStrip { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };
            statusLabel = new ToolStripStatusLabel("Ready") { ForeColor = Color.White };
            status.Items.Add(statusLabel);
            Controls.Add(status);

            // ===== MAIN SPLIT =====
            var mainSplit = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 250, BackColor = Color.FromArgb(30, 30, 30) };
            Controls.Add(mainSplit);
            mainSplit.BringToFront();

            // ===== PROJECT TREE =====
            projectTree = new TreeView
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38),
                ForeColor = Color.White
            };
            projectTree.NodeMouseDoubleClick += (_, e) =>
            {
                if (File.Exists(e.Node.Tag?.ToString()))
                    OpenFileTab(e.Node.Tag!.ToString()!);
            };
            mainSplit.Panel1.Controls.Add(projectTree);
            LoadProjectTree();

            // ===== EDITOR / PREVIEW SPLIT =====
            var editorSplit = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 650, BackColor = Color.FromArgb(30, 30, 30) };
            mainSplit.Panel2.Controls.Add(editorSplit);

            tabs = new TabControl { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White };
            editorSplit.Panel1.Controls.Add(tabs);

            preview = new WebView2 { Dock = DockStyle.Fill };
            editorSplit.Panel2.Controls.Add(preview);

            preview.CoreWebView2InitializationCompleted += (_, __) =>
            {
                ReloadPreview();
            };

            preview.EnsureCoreWebView2Async();
        }

        // ===== PROJECT TREE =====
        private void LoadProjectTree()
        {
            projectTree.Nodes.Clear();
            var root = new TreeNode(Path.GetFileName(GameDir!)) { Tag = GameDir! };
            LoadFilesRecursive(GameDir!, root);
            projectTree.Nodes.Add(root);
            root.Expand();
        }

        private void LoadFilesRecursive(string path, TreeNode node)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var dirNode = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                    LoadFilesRecursive(dir, dirNode);
                    node.Nodes.Add(dirNode);
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    node.Nodes.Add(new TreeNode(Path.GetFileName(file)) { Tag = file });
                }
            }
            catch { /* skip folders without permission */ }
        }

        // ===== FILE TABS =====
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
                BackColor = Color.FromArgb(37, 37, 38),
                ForeColor = Color.White,
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

        // ===== PREVIEW =====
        private void ReloadPreview()
        {
            if (preview.CoreWebView2 == null) return;
            string page = Path.Combine(GameDir!, "run.html");
            if (!File.Exists(page))
            {
                File.WriteAllText(page, "<!DOCTYPE html><html><body><h1>Run.html not found</h1></body></html>");
            }
            preview.CoreWebView2.Navigate(page);
            statusLabel.Text = "Preview refreshed";
        }

        // ===== OPEN FOLDER =====
        private void OpenFolder()
        {
            System.Diagnostics.Process.Start("explorer", GameDir!);
        }

        // ===== ASSET IMPORTER =====
        private void ImportAsset()
        {
            using var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "Import Asset(s)";
            if (dialog.ShowDialog() != DialogResult.OK) return;

            foreach (var file in dialog.FileNames)
            {
                var assetsDir = Path.Combine(GameDir!, "Assets");
                Directory.CreateDirectory(assetsDir);
                var dest = Path.Combine(assetsDir, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }

            LoadProjectTree();
            statusLabel.Text = "Assets imported";
        }
    }
}