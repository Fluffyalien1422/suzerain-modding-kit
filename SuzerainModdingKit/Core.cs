using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "1.0.0", "Fluffyalien", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]

namespace SuzerainModdingKit;

//TODO: fix crash once step is completed (likely from one of the patches in Events.cs)

internal sealed class Core : MelonMod
{
    private bool _showDebugOverlay;

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Cleaning up mod saves.");
        Save.ModSaveCleanup.CleanupOrphanedModSaves();
        LoggerInstance.Msg("Initialized.");

        Variables.Register("SuzerainModdingKit.TestVar");
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

        string stepName = null;
        try
        {
            stepName = Managers.Instance.GameFlowManager.currentStepData.NameInDatabase;
        }
        catch
        {
            // GameFlowManager may not be loaded.
        }

        Event e = Event.current;

        Rect overlayRect = new(30, 200, 220, 300);
        GUI.Box(overlayRect, "Debug Overlay");
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
                string newStepName = Managers.Instance.GameFlowManager.currentStepData.NameInDatabase;
                LoggerInstance.Msg($"Debug overlay: Skipped step '{stepName}'. New step: '{newStepName}'.");
            }
            if (GUI.Button(new Rect(40, 350, 200, 30), "Next Turn"))
            {
                LoggerInstance.Msg("Debug overlay: Showing next turn button.");
                Managers.Instance.GameFlowManager.ShowEndTurnButton();
            }
        }

        // cancel the event before it reaches the game
        // so you can't click through the overlay
        if (overlayRect.Contains(e.mousePosition) && e.isMouse && GUIUtility.hotControl == 0)
        {
            e.Use();
        }
    }
}
