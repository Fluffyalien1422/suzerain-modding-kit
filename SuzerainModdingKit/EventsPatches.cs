using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit;

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EvaluateCurrentStep))]
internal static class GameFlowManager_EvaluateCurrentStep_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("Event: OnEvaluateStep.");
        Events.TriggerOnEvaluateStep();
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndStep))]
internal static class GameFlowManager_EndStep_Patch
{
    // using Postfix crashes the game.
    public static void Prefix()
    {
        Melon<Core>.Logger.Msg("Event: OnStepEnd.");
        Events.TriggerOnStepEnd();
    }
}

[HarmonyPatch(typeof(GameFlowManager), nameof(GameFlowManager.EndTurn))]
internal static class GameFlowManager_EndTurn_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg("Event: OnTurnEnd.");
        Events.TriggerOnTurnEnd();
    }
}

[HarmonyPatch(typeof(BillPanel), nameof(BillPanel.SignBill))]
internal static class BillPanel_SignBill_Patch
{
    public static void Postfix(BillPanel __instance)
    {
        string billName = __instance.currentBillData.NameInDatabase;
        Melon<Core>.Logger.Msg($"Event: OnBillSigned ({billName}).");
        Events.TriggerOnBillSigned(billName);
    }
}

[HarmonyPatch(typeof(BillPanel), nameof(BillPanel.VetoBill))]
internal static class BillPanel_VetoBill_Patch
{
    public static void Postfix(BillPanel __instance)
    {
        string billName = __instance.currentBillData.NameInDatabase;
        Melon<Core>.Logger.Msg($"Event: OnBillVetoed ({billName}).");
        Events.TriggerOnBillVetoed(billName);
    }
}

[HarmonyPatch(typeof(JournalDecisionsPage), nameof(JournalDecisionsPage.CreateJournalTurn))]
internal static class JournalDecisionsPage_CreateJournalTurn_Patch
{
    public static void Postfix(int turnNo)
    {
        // Suzerain creates each journal turn in descending order. Once we reach turn 1,
        // all other turns have been created. Note that this is called every turn,
        // and may be called twice.
        if (turnNo != 1)
        {
            return;
        }

        Melon<Core>.Logger.Msg("Event: OnJournalInitialized.");
        Events.TriggerOnJournalInitialized();
    }
}
