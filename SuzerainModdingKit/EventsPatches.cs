using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit;

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EvaluateCurrentStep))]
internal static class GameFlowManager_EvaluateCurrentStep_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("Event: OnEvaluateStep (GameFlowManager.EvaluateCurrentStep).");
        Events.TriggerOnEvaluateStep();
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndStep))]
internal static class GameFlowManager_EndStep_Patch
{
    // using Postfix crashes the game.
    public static void Prefix()
    {
        Melon<Core>.Logger.Msg("Event: OnStepEnd (GameFlowManager.EndStep).");
        Events.TriggerOnStepEnd();
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndTurn))]
internal static class GameFlowManager_EndTurn_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("Event: OnTurnEnd (GameFlowManager.EndTurn).");
        Events.TriggerOnTurnEnd();
    }
}

[HarmonyPatch(typeof(BillPanel), nameof(BillPanel.SignBill))]
internal static class BillPanel_SignBill_Patch
{
    public static void Postfix(BillPanel __instance)
    {
        Melon<Core>.Logger.Msg("Event: OnBillSigned (BillPanel.SignBill).");
        string billName = __instance.currentBillData.NameInDatabase;
        Events.TriggerOnBillSigned(billName);
    }
}

[HarmonyPatch(typeof(BillPanel), nameof(BillPanel.VetoBill))]
internal static class BillPanel_VetoBill_Patch
{
    public static void Postfix(BillPanel __instance)
    {
        Melon<Core>.Logger.Msg("Event: OnBillVetoed (BillPanel.VetoBill).");
        string billName = __instance.currentBillData.NameInDatabase;
        Events.TriggerOnBillVetoed(billName);
    }
}
