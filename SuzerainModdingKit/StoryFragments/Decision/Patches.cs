using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit.StoryFragments.Decision;

[HarmonyPatch(typeof(DecisionPanel), nameof(DecisionPanel.Show))]
internal static class DecisionPanel_Show_Patch
{
    // Use Prefix to modify the decision before it shows.
    public static void Prefix(DecisionPanel __instance)
    {
        string decisionName = __instance.currentDecisionData.NameInDatabase;
        DecisionShowContext context = new(decisionName);

        Melon<Core>.Logger.Msg($"Event: BeforeDecisionShow ({decisionName}).");
        Events.TriggerBeforeDecisionShow(context);
    }
}
