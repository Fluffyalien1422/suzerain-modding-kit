namespace SuzerainModdingKit.Save;

internal static class ModSaveCleanup
{
    public static void CleanupOrphanedModSaves()
    {
        if (!Directory.Exists(ModdingKitConstants.ModSavePath))
        {
            return;
        }

        HashSet<string> suzerainSaveNames = Directory
            .GetFiles(ModdingKitConstants.SuzerainSavePath, "*.json", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        string[] modSavePaths = Directory.GetFiles(ModdingKitConstants.ModSavePath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (string filePath in modSavePaths)
        {
            string fileName = Path.GetFileName(filePath);
            if (!suzerainSaveNames.Contains(fileName))
            {
                File.Delete(filePath);
            }
        }
    }
}
