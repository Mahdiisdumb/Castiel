using System;
using System.IO;
using System.Windows.Forms;

namespace Castiel
{
public partial class MainForm : Form
    {
        Button pick, make, mod;

        public MainForm()
        {
            Text = "Castiel SDK";
            Width = 400;
            Height = 200;

            pick = new Button { Text = "Pick SDSG Path", Dock = DockStyle.Top };
            make = new Button { Text = "Make Game", Dock = DockStyle.Top };
            mod  = new Button { Text = "Mod Game", Dock = DockStyle.Top };

            pick.Click += PickPath;
            make.Click += MakeGame;
            mod.Click  += ModGame;

            Controls.Add(mod);
            Controls.Add(make);
            Controls.Add(pick);

            Rivalry.Start(); // start random popups about Harris
        }

        private void PickPath(object s, EventArgs e)
        {
            using var f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Config.SDSGPath = f.SelectedPath;
                Config.Save();
            }
        }

        private void MakeGame(object s, EventArgs e)
        {
            if (Config.SDSGPath == null) return;

            string dir = Path.Combine(Config.SDSGPath, "src", "pai", "assets", "NewGame");
            Directory.CreateDirectory(dir);

            File.WriteAllText(Path.Combine(dir, "run.html"), Templates.RunHtml);
            File.WriteAllText(Path.Combine(dir, "style.css"), Templates.StyleCss);
            File.WriteAllText(Path.Combine(dir, "game.js"), Templates.GameJs);
            File.WriteAllText(Path.Combine(dir, "sdk.js"), Templates.SdkJs);

            new EditorForm(dir).Show();
        }

        private void ModGame(object? sender, EventArgs e) {
        
            using var f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(Path.Combine(f.SelectedPath, "sdk.js")))
                {
                    MessageBox.Show(
                        "WARNING\nThis is either trash SDK or pure JS.\nMahdi spaghetti code detected.",
                        "Castiel",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                new EditorForm(f.SelectedPath).Show();
            }
        }
    }
}