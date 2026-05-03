using System.Collections.ObjectModel;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// Static interface for registering <c cref="ConversationInjection">ConversationInjections</c>.
/// </summary>
public static class ConversationRegistry
{
    private static readonly List<ConversationInjection> _injections = [];
    /// <summary>
    /// Read-only list of registered <c cref="ConversationInjection">ConversationInjections</c>.
    /// </summary>
    public static ReadOnlyCollection<ConversationInjection> Injections
    {
        get;
    } = new(_injections);

    /// <summary>
    /// If `true`, no new registrations will be accepted.
    /// </summary>
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

    /// <summary>
    /// Register a new <c cref="ConversationInjection">ConversationInjection</c>.
    /// </summary>
    /// <param name="injection">
    /// The <c cref="ConversationInjection">ConversationInjection</c> to register.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if registration is closed.
    /// </exception>
    public static void RegisterInjection(ConversationInjection injection)
    {
        ArgumentNullException.ThrowIfNull(injection);
        ThrowIfClosed();

        // Clone the injection to ensure it isn't modified after registration.
        _injections.Add(injection.Clone());
    }
}
