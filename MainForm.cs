using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic; // for InputBox
using System.Drawing;

namespace Castiel
{
    public partial class MainForm : Form
    {
        private ListBox actions;
        private ToolStripStatusLabel statusLabel;

        public MainForm()
        {
            Text = "Castiel SDK";
            Width = 1000;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(30, 30, 30);

            // ===== MENU BAR =====
            var menu = new MenuStrip { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };

            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Set SDSG Path", null, (_, __) => PickPath());
            fileMenu.DropDownItems.Add("Exit", null, (_, __) => Close());

            var projectMenu = new ToolStripMenuItem("Project");
            projectMenu.DropDownItems.Add("New Game", null, (_, __) => MakeGame());
            projectMenu.DropDownItems.Add("Open Existing Game", null, (_, __) => ModGame());

            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("About Castiel", null, (_, __) =>
                MessageBox.Show("Castiel SDK\nFor SDSG", "About"));

            menu.Items.Add(fileMenu);
            menu.Items.Add(projectMenu);
            menu.Items.Add(helpMenu);
            MainMenuStrip = menu;
            Controls.Add(menu);

            // ===== STATUS BAR =====
            var status = new StatusStrip { BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };
            statusLabel = new ToolStripStatusLabel("No SDSG path selected") { ForeColor = Color.White };
            status.Items.Add(statusLabel);
            Controls.Add(status);

            // ===== SPLIT VIEW =====
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 220,
                FixedPanel = FixedPanel.Panel1,
                BackColor = Color.FromArgb(30, 30, 30)
            };
            Controls.Add(split);
            split.BringToFront();

            // ===== LEFT PANEL =====
            actions = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38),
                ForeColor = Color.White
            };
            actions.Items.Add("New Game");
            actions.Items.Add("Open Game");
            actions.SelectedIndexChanged += (s, e) =>
            {
                switch (actions.SelectedItem?.ToString())
                {
                    case "New Game": MakeGame(); break;
                    case "Open Game": ModGame(); break;
                }
                actions.ClearSelected();
            };
            split.Panel1.Controls.Add(actions);

            // ===== RIGHT PANEL =====
            var welcome = new Label
            {
                Text = "Castiel SDK\n\nSelect an action from the left panel.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White
            };
            split.Panel2.Controls.Add(welcome);

            // ===== Load config and update status =====
            Config.Load();
            if (!string.IsNullOrWhiteSpace(Config.SDSGPath))
                statusLabel.Text = $"SDSG Path: {Config.SDSGPath}";

            // ===== Start Rivalry (unhinged popups) =====
            Rivalry.Start();
        }

        private void PickPath()
        {
            using var f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Config.SDSGPath = f.SelectedPath;
                Config.Save();
                statusLabel.Text = $"SDSG Path: {Config.SDSGPath}";
            }
        }

        private void MakeGame()
        {
            if (string.IsNullOrWhiteSpace(Config.SDSGPath))
            {
                MessageBox.Show("Pick the SDSG path first!");
                return;
            }

            string gameName = Interaction.InputBox("Enter new game name:", "Castiel SDK", "MyGame");
            if (string.IsNullOrWhiteSpace(gameName)) return;

            foreach (char c in Path.GetInvalidFileNameChars())
                if (gameName.Contains(c))
                {
                    MessageBox.Show("Invalid game name.");
                    return;
                }

            string dir = Path.Combine(Config.SDSGPath, "src", "pai", "assets", gameName);
            if (Directory.Exists(dir))
            {
                MessageBox.Show("That game already exists.");
                return;
            }

            Directory.CreateDirectory(dir);

            // Ask user if they want to use the default template
            var result = MessageBox.Show(
                "Do you want to use the default Castiel game template?",
                "Choose Template",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Cancel)
                return;

            if (result == DialogResult.Yes)
            {
                // Use the full template
                File.WriteAllText(Path.Combine(dir, "run.html"), Templates.RunHtml);
                File.WriteAllText(Path.Combine(dir, "style.css"), Templates.StyleCss);
                File.WriteAllText(Path.Combine(dir, "game.js"), Templates.GameJs);
                File.WriteAllText(Path.Combine(dir, "sdk.js"), Templates.SdkJs);
            }
            else
            {
                // Empty starter files if user wants to start from scratch
                File.WriteAllText(Path.Combine(dir, "run.html"), "<!-- Your HTML here -->");
                File.WriteAllText(Path.Combine(dir, "style.css"), "/* Your CSS here */");
                File.WriteAllText(Path.Combine(dir, "game.js"), "// Your JS here");
                File.WriteAllText(Path.Combine(dir, "sdk.js"), Templates.SdkJs);
            }

            new EditorForm(dir).Show();
        }


        private void ModGame()
{
    using var f = new FolderBrowserDialog();
    if (f.ShowDialog() == DialogResult.OK)
    {
        var sdkPath = Path.Combine(f.SelectedPath, "sdk.js");

        if (!File.Exists(sdkPath))
        {
            MessageBox.Show(
                "WARNING: This game was created without the Castiel SDK.\nSome functionality may not work.",
                "Castiel SDK Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        new EditorForm(f.SelectedPath).Show();
    }
}
    }
}
