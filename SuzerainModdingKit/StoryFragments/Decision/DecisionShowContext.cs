using Il2Cpp;

namespace SuzerainModdingKit.StoryFragments.Decision;

// Sealed because the constructor is internal anyway.
public sealed class DecisionShowContext
{
    public string DecisionName
    {
        get;
    }

    internal DecisionShowContext(string decisionName)
    {
        DecisionName = decisionName ?? throw new ArgumentNullException(nameof(decisionName));
    }

    private void ThrowIfInvalid()
    {
        if (!IsValid())
        {
            throw new InvalidOperationException("Context is invalid.");
        }
    }

    public bool IsValid()
    {
        //TODO: this still returns true even after the player has selected their option.
        DecisionPanel panel = Panels.Instance?.DecisionPanel;
        if (panel == null)
        {
            return false;
        }
        string currentName = panel.currentDecisionData?.NameInDatabase;
        return DecisionName.Equals(currentName, StringComparison.Ordinal);
    }

    public void AddOption(string text)
    {
        ThrowIfInvalid();

        DecisionPanel panel = Panels.Instance.DecisionPanel;

        int nextIndex = panel.instantiatedDecisionOptionButtons.Count;
        DecisionProperties.DecisionOption option = new()
        {
            Text = text,
        };

        panel.CreateDecisionOptionButton(option, nextIndex);
    }
}
