using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit;

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EvaluateCurrentStep))]
internal static class GameFlowManager_EvaluateCurrentStep_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("Lifecycle event: OnEvaluateStep (GameFlowManager.EvaluateCurrentStep).");
        Events.TriggerOnEvaluateStep();
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndStep))]
internal static class GameFlowManager_EndStep_Patch
{
    // using Postfix crashes the game.
    public static void Prefix()
    {
        Melon<Core>.Logger.Msg("Lifecycle event: OnStepEnd (GameFlowManager.EndStep).");
        Events.TriggerOnStepEnd();
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndTurn))]
internal static class GameFlowManager_EndTurn_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("Lifecycle event: OnTurnEnd (GameFlowManager.EndTurn).");
        Events.TriggerOnTurnEnd();
    }
}
