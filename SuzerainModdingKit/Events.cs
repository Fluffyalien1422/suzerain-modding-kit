namespace SuzerainModdingKit;

public static class Events
{
    public static event EventHandler OnEvaluateStep;
    public static event EventHandler OnStepEnd;
    public static event EventHandler OnTurnEnd;
    public static event EventHandler<BillEventArgs> OnBillSigned;
    public static event EventHandler<BillEventArgs> OnBillVetoed;
    public static event EventHandler OnJournalInitialized;

    public class BillEventArgs : EventArgs
    {
        public string BillName
        {
            get; init;
        }

        public BillEventArgs(string billName)
        {
            BillName = billName ?? throw new ArgumentNullException(nameof(billName));
        }
    }

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

    internal static void TriggerOnBillSigned(string billName)
    {
        OnBillSigned?.Invoke(sender: null, new BillEventArgs(billName));
    }

    internal static void TriggerOnBillVetoed(string billName)
    {
        OnBillVetoed?.Invoke(sender: null, new BillEventArgs(billName));
    }

    internal static void TriggerOnJournalInitialized()
    {
        OnJournalInitialized?.Invoke(sender: null, EventArgs.Empty);
    }
}
