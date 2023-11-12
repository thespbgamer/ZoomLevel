using System;
using System.IO;
using StardewModdingAPI;

namespace ZoomLevel
{
    public partial class ModEntry
    {
        private class CommonHelper
        {
            internal static void RemoveObsoleteFiles(IMod mod, params string[] relativePaths)
            {
                string basePath = mod.Helper.DirectoryPath;

                foreach (string relativePath in relativePaths)
                {
                    string fullPath = Path.Combine(basePath, relativePath);
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            File.Delete(fullPath);
                            mod.Monitor.Log($"Removed obsolete file '{relativePath}'.");
                        }
                        catch (Exception ex)
                        {
                            mod.Monitor.Log($"Failed deleting obsolete file '{relativePath}':\n{ex}");
                        }
                    }
                }
            }
        }
    }
}