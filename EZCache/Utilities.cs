using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using MelonLoader;
using ABI_RC.Core.InteractionSystem;
using System.Collections;
using Newtonsoft.Json;

namespace EZCache
{
    public class EZCacheSetting
    {
        public readonly string DisplayName;

        public object Value { get; set; }

        public EZCacheSetting(string displayName, object value)
        {
            DisplayName = displayName;
            Value = value;
        }
    }

    internal static class Utilities
    {
        private static readonly Dictionary<string, EZCacheSetting> defaultSettings = new()
        {
            { "aviCache", new EZCacheSetting("Avatar Cache Max Size", 1) },
            { "propCache", new EZCacheSetting("Prop Cache Max Size", 1) },
            { "wrldCache", new EZCacheSetting("World Cache Max Size", 1) }
        };

        public static Config config;
        public static Dictionary<string, EZCacheSetting> settings;

        public static string
            avatarCachePath = "ChilloutVR_Data\\Avatars",
            propCachePath = "ChilloutVR_Data\\Spawnables",
            worldCachePath = "ChilloutVR_Data\\Worlds",
            ezCacheDataDir = "UserData\\EZCache",
            configPath = ezCacheDataDir + "\\config.json";

        private static void IntegrityCheck()
        {
            if (!Directory.Exists(ezCacheDataDir))
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "Failed to find the EZCache data directory, initializing now....");
                Directory.CreateDirectory(ezCacheDataDir);
                config = new()
                {
                    settings = defaultSettings
                };
                string jsonData = JsonConvert.SerializeObject(config);
                File.WriteAllText(configPath, jsonData);
                settings = config.settings;
                MelonLogger.Msg(ConsoleColor.Green, "Initialized new config!");
            }
        }

        public static void SaveConfig()
        {
            IntegrityCheck();
            string jsonData = JsonConvert.SerializeObject(config);
            File.WriteAllText(configPath, jsonData);
        }

        public static void LoadConfig()
        {
            MelonLogger.Msg(ConsoleColor.Blue, "Loading config...");
            IntegrityCheck();
            string fileData = File.ReadAllText(configPath);
            config = JsonConvert.DeserializeObject<Config>(fileData);
            settings = config.settings;
            MelonLogger.Msg(ConsoleColor.Green, "Loaded config!");
        }

        public static void AddValue(string valueName, int value)
        {
            var setting = settings[valueName];

            if (Convert.ToInt32(setting.Value) + value < 1)
            {
                ViewManager.Instance.TriggerAlert("EZCache", $"Cannot set {setting.DisplayName.ToLower()} to less than 1GB!", -1, true);
                return;
            }

            settings[valueName].Value = Convert.ToInt32(setting.Value) + value;
            MelonLogger.Msg($"Adding {value} to {valueName}.");
            ViewManager.Instance.quickMenuView.View.ExecuteScript(@"﻿{

            var cacheSizeDisplay = document.getElementById('EZCache_" + valueName + @"SizeDisplay');
            cacheSizeDisplay.innerHTML = '" + setting.DisplayName + ": " + Convert.ToInt32(setting.Value) + @"GB';

            }");
            SaveConfig();
        }

        public static void RefreshCacheValues()
        {
            var aviCacheDir = new DirectoryInfo(avatarCachePath);
            double aviCacheSize = Math.Round((double)aviCacheDir.EnumerateFiles().Sum(file => file.Length) / 1000000000, 2);

            var propCacheDir = new DirectoryInfo(propCachePath);
            double propCacheSize = Math.Round((double)propCacheDir.EnumerateFiles().Sum(file => file.Length) / 1000000000, 2);

            var wrldCacheDir = new DirectoryInfo(worldCachePath);
            double wrldCacheSize = Math.Round((double)wrldCacheDir.EnumerateFiles().Sum(file => file.Length) / 1000000000, 2);

            MelonLogger.Msg($"{aviCacheDir}: {aviCacheSize}");
            MelonLogger.Msg($"{propCacheDir}: {propCacheSize}");
            MelonLogger.Msg($"{wrldCacheDir}: {wrldCacheSize}");

            ViewManager.Instance.quickMenuView.View.ExecuteScript(@"﻿{

            var aviCacheDisplay = document.getElementById('EZCache_aviCacheDisplay');
            var propCacheDisplay = document.getElementById('EZCache_propCacheDisplay');
            var wrldCacheDisplay = document.getElementById('EZCache_wrldCacheDisplay');
            aviCacheDisplay.innerHTML = 'Current avatar cache size: " + aviCacheSize.ToString() + @"GB';
            propCacheDisplay.innerHTML = 'Current prop cache size: " + propCacheSize.ToString() + @"GB';
            wrldCacheDisplay.innerHTML = 'Current world cache size: " + wrldCacheSize.ToString() + @"GB';

            }");
        }

        /// <summary>
        /// Iterated through and deletes all files in all cache directories.
        /// </summary>
        public static void ClearAllCache()
        {
            ClearAvatarCache(false);
            ClearPropCache(false);
            ClearWorldCache(false);
            ViewManager.Instance.TriggerAlert("EZCache", "Cleared all cache!", -1, true);
        }

        /// <summary>
        /// Iterates through and deleted all files in the Avatars directory.
        /// </summary>
        public static void ClearAvatarCache(bool indieCall)
        {
            MelonLogger.Msg(ConsoleColor.Blue, "Clearing avatar cache...");

            DirectoryInfo dirInfo = new DirectoryInfo(avatarCachePath);
            FileInfo[] dirFiles = dirInfo.GetFiles();

            for (int i = 0; i < dirFiles.Length; ++i)
            {
                try
                {
                    dirFiles[i].Delete();
                }
                catch
                {
                    MelonLogger.Msg(ConsoleColor.Yellow, $"Skipping {dirFiles[i].Name}..");
                    continue;
                }

            }
            if (indieCall) ViewManager.Instance.TriggerAlert("EZCache", "Cleared avatar cache!", -1, true);
            MelonLogger.Msg(ConsoleColor.Green, $"Cleared avatar cache!");
        }

        /// <summary>
        /// Iterates through and deletes all files in the Spawnables directory.
        /// </summary>
        public static void ClearPropCache(bool indieCall)
        {
            MelonLogger.Msg(ConsoleColor.Blue, "Clearing prop cache...");

            DirectoryInfo dirInfo = new DirectoryInfo(propCachePath);
            FileInfo[] dirFiles = dirInfo.GetFiles();

            for (int i = 0; i < dirFiles.Length; ++i)
            {
                try
                {
                    dirFiles[i].Delete();
                }
                catch
                {
                    MelonLogger.Msg(ConsoleColor.Yellow, $"Skipping {dirFiles[i].Name}..");
                    continue;
                }

            }
            if (indieCall) ViewManager.Instance.TriggerAlert("EZCache", "Cleared prop cache!", -1, true);
            MelonLogger.Msg(ConsoleColor.Green, $"Cleared prop cache!");
        }

        /// <summary>
        /// Iterates through and deleted all files in the Worlds directory.
        /// </summary>
        public static void ClearWorldCache(bool indieCall)
        {
            MelonLogger.Msg(ConsoleColor.Blue, "Clearing world cache...");

            DirectoryInfo dirInfo = new DirectoryInfo(worldCachePath);
            FileInfo[] dirFiles = dirInfo.GetFiles();

            for (int i = 0; i < dirFiles.Length; ++i)
            {
                try
                {
                    dirFiles[i].Delete();
                }
                catch
                {
                    MelonLogger.Msg(ConsoleColor.Yellow, $"Skipping {dirFiles[i].Name}..");
                    continue;
                }

            }
            if (indieCall) ViewManager.Instance.TriggerAlert("EZCache", "Cleared world cache!", -1, true);
            MelonLogger.Msg(ConsoleColor.Green, $"Cleared world cache!");
        }

        /// <summary>
        /// Starts game with the arguments it was originally started with and kills current process.
        /// </summary>
        public static void RestartGame()
        {
            Process.Start("ChilloutVR.exe", Environment.CommandLine);
            Process.GetCurrentProcess().Kill();
            SaveConfig();
        }
    }
}
