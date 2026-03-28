using MelonLoader;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "1.0.0", "Fluffyalien", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]

namespace SuzerainModdingKit;

internal class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Cleaning up mod saves.");
        Save.ModSaveCleanup.CleanupOrphanedModSaves();
        LoggerInstance.Msg("Initialized.");
    }
}