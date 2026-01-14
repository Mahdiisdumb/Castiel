using System;
using System.IO;
using System.Text.Json;

namespace Castiel
{
    static class Config
    {
        private static readonly string file =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static string? SDSGPath; // nullable to avoid CS8618

        public static void Load()
        {
            if (!File.Exists(file)) return;
            try
            {
                SDSGPath = JsonSerializer.Deserialize<string>(File.ReadAllText(file));
            }
            catch
            {
                SDSGPath = null;
            }
        }

        public static void Save()
        {
            File.WriteAllText(file, JsonSerializer.Serialize(SDSGPath));
        }
    }
}