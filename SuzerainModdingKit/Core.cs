using MelonLoader;
using SuzerainModdingKit.Save;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "0.1.0", "Fluffyalien", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]
// (also update version in ModdingKitConstants.cs)

namespace SuzerainModdingKit;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg(
            $"Suzerain Modding Kit version: {ModdingKitConstants.ModVersion}, " +
            $"Suzerain version: {Application.version}, " +
            $"Target Suzerain version: {ModdingKitConstants.TargetSuzerainVersion}.");

        if (!string.Equals(
            Application.version,
            ModdingKitConstants.TargetSuzerainVersion,
            StringComparison.Ordinal))
        {
            LoggerInstance.Warning(
                $"Expected Suzerain version {ModdingKitConstants.TargetSuzerainVersion}, " +
                $"but got {Application.version}. Suzerain Modding Kit may not work properly.");
        }

        LoggerInstance.Msg("Cleaning up mod saves.");
        SaveManager.CleanupOrphanedModSaves();

        LoggerInstance.Msg("Initialized.");
    }

    public override void OnUpdate()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null)
        {
            return;
        }

        if (kb.ctrlKey.isPressed && kb.dKey.wasPressedThisFrame)
        {
            DebugOverlay.SetVisibility(value: !DebugOverlay.IsShowing());
        }
    }

    public override void OnGUI()
    {
        DebugOverlay.Update();
    }
}
