using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit;

//public static class Events
//{

//}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EvaluateCurrentStep))]
internal static class GameFlowManager_EvaluateCurrentStep_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("GameFlowManager_EvaluateCurrentStep");
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndStep))]
internal static class GameFlowManager_EndStep_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("GameFlowManager_EndStep");
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndTurn))]
internal static class GameFlowManager_EndTurn_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("GameFlowManager_EndTurn");
    }
}
