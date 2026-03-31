using Il2Cpp;

namespace SuzerainModdingKit;

public static class GameState
{
    public static int? CurrentStepNum => Managers.Instance?.GameFlowManager?.CurrentStepNo;
    public static string CurrentStepName => Managers.Instance?.GameFlowManager?.currentStepData?.NameInDatabase;
    public static int? CurrentTurnNum => Managers.Instance?.GameFlowManager?.CurrentTurnNo;
    public static string CurrentTurnName => Managers.Instance?.GameFlowManager?.currentTurnData?.NameInDatabase;
}
