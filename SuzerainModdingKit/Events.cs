namespace SuzerainModdingKit;

public static class Events
{
    public static event EventHandler OnEvaluateStep;
    public static event EventHandler OnStepEnd;
    public static event EventHandler OnTurnEnd;

    internal static void TriggerOnEvaluateStep()
    {
        OnEvaluateStep?.Invoke(sender: null, EventArgs.Empty);
    }

    internal static void TriggerOnStepEnd()
    {
        OnStepEnd?.Invoke(sender: null, EventArgs.Empty);
    }

    internal static void TriggerOnTurnEnd()
    {
        OnTurnEnd?.Invoke(sender: null, EventArgs.Empty);
    }
}
