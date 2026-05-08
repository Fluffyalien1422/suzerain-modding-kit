using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit.StoryFragments.Bill;

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
