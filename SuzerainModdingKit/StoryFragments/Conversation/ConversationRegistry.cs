namespace SuzerainModdingKit.StoryFragments.Conversation;

public static class ConversationRegistry
{
    private static readonly List<ConversationInjection> _injections = [];
    public static IReadOnlyList<ConversationInjection> Injections => _injections;

    public static bool IsRegistrationClosed
    {
        get; private set;
    }

    private static void ThrowIfClosed()
    {
        if (IsRegistrationClosed)
        {
            throw new InvalidOperationException("Registration is closed.");
        }
    }

    internal static void CloseRegistration()
    {
        IsRegistrationClosed = true;
    }

    public static bool RegisterInjection(ConversationInjection injection)
    {
        ArgumentNullException.ThrowIfNull(injection);
        ThrowIfClosed();

        _injections.Add(injection);
        return true;
    }
}
