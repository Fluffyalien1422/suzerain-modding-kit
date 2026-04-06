using Il2Cpp;
using MelonLoader;
using SuzerainModdingKit.StoryFragments;

namespace SuzerainModdingKit;

public static class GameState
{
    public static int? CurrentStepNum => Managers.Instance?.GameFlowManager?.CurrentStepNo;
    public static string CurrentStepName => Managers.Instance?.GameFlowManager?.currentStepData
        ?.NameInDatabase;
    public static int? CurrentTurnNum => Managers.Instance?.GameFlowManager?.CurrentTurnNo;
    public static string CurrentTurnName => Managers.Instance?.GameFlowManager?.currentTurnData
        ?.NameInDatabase;

    public static bool IsGameActive()
    {
        return CurrentStepName != null;
    }

    public static bool StoryFragmentExistsInCurrentStep(string name)
    {
        return Managers.Instance?.GameFlowManager?.currentStepData?.StoryFragments?.Contains(name)
            ?? false;
    }

    public static void AddCustomBill(CustomBillData customBillData)
    {
        // This method adds a custom bill to the current step dynamically.
        // We don't use Suzerain's registry because Suzerain doesn't recognize
        // custom variables in conditions. Adding story fragments dynamically
        // also allows more control for modders.

        if (!IsGameActive())
        {
            throw new InvalidOperationException(
                "Cannot add a story fragment when the game is not active.");
        }

        if (StoryFragmentExistsInCurrentStep(customBillData.Name))
        {
            throw new InvalidOperationException(
                $"A story fragment with the name '{customBillData.Name}' already exists in the current step.");
        }

        GameFlowManager gameFlowManager = Managers.Instance.GameFlowManager;
        BillData billData = customBillData.ToSuzerainBillData();

        // Check if the bill data already exists in the registry.
        Func<BillData, bool> match = d => string.Equals(
            d.NameInDatabase,
            customBillData.Name,
            StringComparison.Ordinal);
        bool existsInRegistry = EntityDataManager.AllBillsData.Exists(match);
        if (!existsInRegistry)
        {
            EntityDataManager.AllBillsData.Add(billData);
        }

        // Add it to the scene and create the token indicator (the exclamation icon).
        gameFlowManager.enabledNotDoneStoryFragments.Add(billData);
        gameFlowManager.currentStepData.StoryFragments.Add(customBillData.Name);
        CreateTokenIndicator(
            customBillData.AssignedTokenName,
            TokenIndicatorPanel.TokenIndicatorType.StoryFragment);
    }

    private static void CreateTokenIndicator(string assignedTokenName,
        TokenIndicatorPanel.TokenIndicatorType indicatorType)
    {
        if (!IsGameActive())
        {
            throw new InvalidOperationException(
                "Cannot add a token indicator when the game is not active.");
        }

        TokenIndicatorPanel panel = Panels.Instance.TokenIndicatorPanel;

        // Check if the token indicator we're trying to create already exists.
        Func<TokenIndicatorPanel.TokenIndicator, bool> match =
            ind => string.Equals(
                ind.token.tokenDataEntityName,
                assignedTokenName,
                StringComparison.Ordinal) && ind.tokenIndicatorType == indicatorType;
        bool exists = panel.tokenIndicators.Exists(match);
        if (exists)
        {
            return;
        }

        // TryAddTokenIndicator creates the token indicator and adds it to the list.
        panel.TryAddTokenIndicator(
            assignedTokenName,
            indicatorType,
            panel.tokenIndicators);

        // Get the indicator we just created.
        TokenIndicatorPanel.TokenIndicator newIndicator = panel.tokenIndicators.Find(match);
        if (newIndicator == null)
        {
            Melon<Core>.Logger.Error(
                $"Failed to create token indicator for token '{assignedTokenName}'.");
            return;
        }

        // Get a template. For some reason, panel.templateTokenIndicator doesn't work,
        // so we just grab the first one from the list of instantiated token indicators.
        TemplateTokenIndicator indicatorTemplate = panel.instantiatedTokenIndicators[0];
        if (indicatorTemplate == null)
        {
            Melon<Core>.Logger.Error(
                $"Failed to create token indicator for token '{assignedTokenName}'. No template object found.");
            return;
        }

        // Create the game object.
        TemplateTokenIndicator indicatorObj = UnityEngine.Object.Instantiate(
            indicatorTemplate,
            indicatorTemplate.transform.parent);
        indicatorObj.Setup(newIndicator);
        panel.instantiatedTokenIndicators.Add(indicatorObj);
    }
}
