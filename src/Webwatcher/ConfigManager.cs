﻿using System;
using System.IO;

namespace Webwatcher
{
    public static class ConfigManager
    {
        internal static ConfigBase Config;
        internal static string ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Webwatcher";
        internal static string DownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
        internal static string ConfigFile = ConfigPath + @"\config.cfg";
        internal static string ConfigURL = "file:///" + Directory.GetCurrentDirectory().Replace('\\', '/') + "/Sites/config_basic.html";
        internal static string AdvancedConfigURL = "file:///" + Directory.GetCurrentDirectory().Replace('\\', '/') + "/Sites/config_advanced.html";
        internal static string AboutURL = "file:///" + Directory.GetCurrentDirectory().Replace('\\', '/') + "/Sites/config_about.html";
        internal static string ChangelogURL = "file:///" + Directory.GetCurrentDirectory().Replace('\\', '/') + "/Sites/changelog.html";
        internal static string ErrorURL = "file:///" + Directory.GetCurrentDirectory().Replace('\\', '/') + "/Sites/load_error.html";
        internal static string DownloadsURL = "file:///" + Directory.GetCurrentDirectory().Replace('\\', '/') + "/Sites/downloads.html";

        static ConfigManager()
        {
            if (!Directory.Exists(ConfigPath)) Directory.CreateDirectory(ConfigPath);

            if (!File.Exists(ConfigFile))
            {
                Config = new ConfigBase("", true, "google", true);

                Save(Config);
            }
            else
            {
                var configString = File.ReadAllLines(ConfigFile);

                foreach (string line in configString)
                {
                    if (line == string.Empty || line.StartsWith("#")) continue;
                    else if (line.StartsWith("homepage=")) Config.Homepage = line.Split('=')[1];
                    else if (line.StartsWith("useDefaultHomepage=")) Config.UseDefaultHomepage = line.Split('=')[1] == "true";
                    else if (line.StartsWith("searchEngine=")) Config.SearchEngine = line.Split('=')[1];
                    else if (line.StartsWith("useHardwareAccel=")) Config.UseHardwareAccel = line.Split('=')[1] == "true";
                    else throw new FormatException(line);
                }
            }
        }

        public static void Load(ConfigBase config)
            => Config = config;

        public static void Save(ConfigBase config)
        {
            Config = config;

            File.WriteAllLines(ConfigFile, new string[] {
                "# Webwatcher configuration file",
                "# Generated " + DateTime.Now,
                "",
                "homepage=" + Config.Homepage,
                "useDefaultHomepage=" + (Config.UseDefaultHomepage ? "true" : "false"),
                "searchEngine=" + config.SearchEngine,
                "useHardwareAccel=" + (Config.UseHardwareAccel ? "true" : "false")
            });
        }
    }
}