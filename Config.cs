using System;
using System.IO;
using System.Text.Json;

namespace Castiel
{
    static class Config
    {
        private static readonly string ConfigDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Castiel");

        private static readonly string ConfigFile =
            Path.Combine(ConfigDir, "config.json");

        public static ConfigData Data { get; private set; } = new();

        public static string? SDSGPath
        {
            get => Data.SDSGPath;
            set => Data.SDSGPath = value;
        }

        public static void Load()
        {
            try
            {
                if (!File.Exists(ConfigFile))
                {
                    Save(); // write defaults
                    return;
                }

                string json = File.ReadAllText(ConfigFile);
                Data = JsonSerializer.Deserialize<ConfigData>(json) ?? new();
            }
            catch
            {
                Data = new(); // reset on corruption
            }
        }

        public static void Save()
        {
            Directory.CreateDirectory(ConfigDir);

            var opts = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(ConfigFile, JsonSerializer.Serialize(Data, opts));
        }
    }

    class ConfigData
    {
        public string? SDSGPath { get; set; }
        public int ConfigVersion { get; set; } = 1;
    }
}