using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using HarmonyLib;
using MelonLoader;
using ABI_RC.Core.IO;
using ABI_RC.Core.Networking.API.Responses;
using System.IO;

namespace EZCache
{
    public class Patches
    {
        private static HarmonyMethod GetLocalPatch(string name) => new HarmonyMethod(typeof(Patches).GetMethod(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static));
        private static HarmonyLib.Harmony Instance;

        public static void ApplyPatches()
        {
            try
            {
                Instance = new("EZCache_Patches");
                Instance.Patch(typeof(CVRDownloadManager).GetMethod("AddDownloadJob"), GetLocalPatch("NewAddDownloadJob"));
            }
            catch (Exception ex)
            {
                MelonLogger.Msg(ex);
            }

        }

        private static bool NewAddDownloadJob(DownloadJob.ObjectType type, string id, string owner = null, string key = null, long size = 0L, bool joinOnComplete = false, string hash = null, string fileID = null, string location = null, UgcTagsData tags = null)
        {
            switch (type)
            {
                case DownloadJob.ObjectType.Avatar:
                    var aviCacheDir = new DirectoryInfo(Utilities.avatarCachePath);
                    bool aviCacheHasFile = aviCacheDir.EnumerateFiles().Any(f => f.Name.Contains(id));
                    if (!aviCacheHasFile) MelonLogger.Msg($"Downloading Avatar | Size: {size.ToString()}");
                    break;
                case DownloadJob.ObjectType.Prop:
                    var propCacheDir = new DirectoryInfo(Utilities.propCachePath);
                    bool propCacheHasFile = propCacheDir.EnumerateFiles().Any(f => f.Name.Contains(id));
                    if (!propCacheHasFile) MelonLogger.Msg($"Downloading Prop | Size: {size.ToString()}");
                    break;
                case DownloadJob.ObjectType.World:
                    var wrldCacheDir = new DirectoryInfo(Utilities.worldCachePath);
                    bool wrldCacheHasFile = wrldCacheDir.EnumerateFiles().Any(f => f.Name.Contains(id));
                    if (!wrldCacheHasFile) MelonLogger.Msg($"Downloading World | Size: {size.ToString()}");
                    break;
            }
            return true;
        }
    }
}
