using SuzerainModdingKit.StoryFragments.Decision;

namespace SuzerainModdingKit;

/// <summary>
/// Suzerain lifecycle events.
/// </summary>
public static class Events
{
    // This class is organized in the following order:
    // 1. Event properties:
    //   1a. Before events. Alphabetical order.
    //   1b. After (On) events. Alphabetical order.
    // 2. EventArgs classes: Same order as properties.
    // 3. Trigger methods: Same order as properties.

    /// <summary>
    /// Called before the game shows a decision. This event can modify the decision.
    /// </summary>
    public static event EventHandler<DecisionShowEventArgs> BeforeDecisionShow;
    /// <summary>
    /// Called before a step ends.
    /// </summary>
    public static event EventHandler BeforeStepEnd;
    /// <summary>
    /// Called when a bill is signed by the player.
    /// </summary>
    public static event EventHandler<BillEventArgs> OnBillSigned;
    /// <summary>
    /// Called when a bill is vetoed by the player.
    /// </summary>
    public static event EventHandler<BillEventArgs> OnBillVetoed;
    /// <summary>
    /// Called when a step is evaluated. Note that this may be called multiple times per step.
    /// </summary>
    public static event EventHandler OnEvaluateStep;
    /// <summary>
    /// Called when the journal is initialized. Note that this may be called multiple times.
    /// </summary>
    public static event EventHandler OnJournalInitialized;
    /// <summary>
    /// Called when a turn ends.
    /// </summary>
    public static event EventHandler OnTurnEnd;

    /// <summary>
    /// Event args passed to <c cref="OnBillSigned">OnBillSigned</c> and
    /// <c cref="OnBillVetoed">OnBillVetoed</c> events.
    /// </summary>
    public class BillEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the bill.
        /// </summary>
        public string BillName
        {
            get;
        }

        public BillEventArgs(string billName)
        {
            BillName = billName ?? throw new ArgumentNullException(nameof(billName));
        }
    }

    public class DecisionShowEventArgs : EventArgs
    {
        public DecisionShowContext Context
        {
            get;
        }

        public DecisionShowEventArgs(DecisionShowContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }

    internal static void TriggerBeforeDecisionShow(DecisionShowContext context)
    {
        BeforeDecisionShow?.Invoke(sender: null, new DecisionShowEventArgs(context));
    }

    internal static void TriggerBeforeStepEnd()
    {
        BeforeStepEnd?.Invoke(sender: null, EventArgs.Empty);
    }

    internal static void TriggerOnBillSigned(string billName)
    {
        OnBillSigned?.Invoke(sender: null, new BillEventArgs(billName));
    }

    internal static void TriggerOnBillVetoed(string billName)
    {
        OnBillVetoed?.Invoke(sender: null, new BillEventArgs(billName));
    }

    internal static void TriggerOnEvaluateStep()
    {
        OnEvaluateStep?.Invoke(sender: null, EventArgs.Empty);
    }

    internal static void TriggerOnJournalInitialized()
    {
        OnJournalInitialized?.Invoke(sender: null, EventArgs.Empty);
    }

    internal static void TriggerOnTurnEnd()
    {
        OnTurnEnd?.Invoke(sender: null, EventArgs.Empty);
    }
}
