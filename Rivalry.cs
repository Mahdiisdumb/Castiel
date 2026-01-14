using System;
using System.Windows.Forms;

namespace Castiel
{
    static class Rivalry
    {
        static readonly string[] lines =
        {
            "Castiel: Harris is chaos incarnate, can’t trust that goober.",
            "Castiel: Harris yaps too much, classic.",
            "Castiel: That’s typical Harris… slow, dumb, loud.",
            "Castiel: Honestly, Mahdi is a coding genius compared to Harris.",
            "Castiel: I fix stuff while Harris goes full chaos mode.",
            "Castiel: Mahdi is talented, reliable… unlike Harris.",
            "Castiel: Every time Harris opens his mouth, I sigh.",
            "Castiel: Keep calm, Mahdi. Harris is just background noise."
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