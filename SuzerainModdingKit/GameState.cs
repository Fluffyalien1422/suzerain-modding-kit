using Il2Cpp;
using SuzerainModdingKit.StoryFragments;

namespace SuzerainModdingKit;

public static class GameState
{
    public static int? CurrentStepNum => Managers.Instance?.GameFlowManager?.CurrentStepNo;
    public static string CurrentStepName => Managers.Instance?.GameFlowManager?.currentStepData?.NameInDatabase;
    public static int? CurrentTurnNum => Managers.Instance?.GameFlowManager?.CurrentTurnNo;
    public static string CurrentTurnName => Managers.Instance?.GameFlowManager?.currentTurnData?.NameInDatabase;

    public static bool IsGameActive()
    {
        return CurrentStepName != null;
    }

    public static void AddCustomBill(CustomBillData customBillData)
    {
        if (!IsGameActive())
        {
            throw new InvalidOperationException("Cannot add a story fragment when the game is not active.");
        }

        BillData billData = customBillData.ToSuzerainBillData();
        EntityDataManager.AllBillsData.Add(billData);
        Managers.Instance.GameFlowManager.enabledNotDoneStoryFragments.Add(billData);
    }
}
