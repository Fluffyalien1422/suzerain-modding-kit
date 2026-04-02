using Il2Cpp;
using MelonLoader;
using SuzerainModdingKit.Save;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "0.1.0", "Fluffyalien", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]
// (also update version in Constants.cs)

namespace SuzerainModdingKit;

internal sealed class Core : MelonMod
{
    private bool _showDebugOverlay;

    public override void OnInitializeMelon()
    {
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
            _showDebugOverlay = !_showDebugOverlay;
        }
    }

    public override void OnGUI()
    {
        if (!_showDebugOverlay)
        {
            return;
        }

        string stepName = GameState.CurrentStepName;

        GUI.Box(new Rect(30, 200, 220, 300), "Debug Overlay");
        GUI.Label(new Rect(30, 230, 200, 30), stepName ?? "GameFlowManager not loaded!");
        if (GUI.Button(new Rect(40, 270, 200, 30), "Hide (Ctrl+D)"))
        {
            _showDebugOverlay = false;
        }
        if (stepName != null)
        {
            if (GUI.Button(new Rect(40, 310, 200, 30), "Next Step"))
            {
                Managers.Instance.GameFlowManager.EndStep();
                string newStepName = GameState.CurrentStepName;
                LoggerInstance.Msg($"Debug overlay: Skipped step '{stepName}'. New step: '{newStepName}'.");
            }
            if (GUI.Button(new Rect(40, 350, 200, 30), "Next Turn"))
            {
                LoggerInstance.Msg("Debug overlay: Showing next turn button.");
                Managers.Instance.GameFlowManager.ShowEndTurnButton();
            }
        }
    }
}
