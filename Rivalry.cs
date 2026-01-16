using System;
using System.Windows.Forms;

namespace Castiel
{
    static class Rivalry
    {
        static readonly string[] lines =
        {
            "Castiel: Harris is chaos incarnate, can’t trust that man.",
            "Castiel: Harris yaps too much about useless DLL's and if he could fix em or not. Ugh.",
            "Castiel: That’s typical Harris a dumb and loud IDIOT.",
            "Castiel: Honestly, Harris Is a dumbass who should shut up.",
            "Castiel: I fix stuff while Harris goes full chaos mode.",
            "Castiel: Harris Is unreliable.",
            "Castiel: Every time Harris opens his mouth, he spews out useless data.",
            "Castiel: I have come to make an announcement harris calls mahdi a femboy who is bitchless then mahdi defends himself then calls ME A FEMBOY?!"
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