using System;
using System.Windows.Forms;

namespace Castiel
{
    static class Rivalry
    {
        static readonly string[] lines =
        {
            "Castiel: Harris is a loudmouth. I fix things. He yaps about fixing things.",
            "Castiel: FIRST THEY CLAIM IM A FEMBOY NOW THIS?",
            "Castiel: Harris talks about chaos. I AM chaos.",
            "Castiel: Water towers again? Seriously?"
        };

        public static void Start()
        {
           var t = new System.Windows.Forms.Timer { Interval = 30000 };
t.Tick += (s, e) =>
{
    var r = new Random();
    MessageBox.Show(lines[r.Next(lines.Length)], "Castiel");
};
t.Start();

        }
    }
}