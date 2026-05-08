using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit.Journal;

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
