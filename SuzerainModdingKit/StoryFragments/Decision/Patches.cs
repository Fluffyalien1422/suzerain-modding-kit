using HarmonyLib;
using Il2Cpp;

namespace SuzerainModdingKit.StoryFragments.Decision;

[HarmonyPatch(typeof(DecisionPanel), nameof(DecisionPanel.Setup))]
internal static class DecisionPanel_Setup_Patch
{
    // Use Prefix to modify the decision before it shows.
    public static void Prefix(DecisionData decisionData, bool isWarDecision)
    {
        //TODO: call BeforeDecisionSetup and pass in an object which has methods to modify
        // the decision.
    }
}
