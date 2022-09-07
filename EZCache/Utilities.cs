using System;
using System.IO;
using System.Diagnostics;

using MelonLoader;
using ABI_RC.Core.InteractionSystem;

namespace EZCache
{
    internal static class Utilities
    {


        public static string
            avatarCachePath = "ChilloutVR_Data\\Avatars",
            propCachePath = "ChilloutVR_Data\\Spawnables",
            worldCachePath = "ChilloutVR_Data\\Worlds";

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
        }
    }
}
