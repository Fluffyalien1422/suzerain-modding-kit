using System.Collections.ObjectModel;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// A list of nodes to be injected into a conversation.
/// </summary>
public class ConversationInjection
{
    private readonly List<ConversationNode> _nodes = [];

    /// <summary>
    /// Read-only list of nodes to be injected.
    /// </summary>
    public ReadOnlyCollection<ConversationNode> Nodes
    {
        get;
    }
    /// <summary>
    /// The conversation title to inject the nodes into.
    /// </summary>
    public string ConversationTitle
    {
        get;
    }
    /// <summary>
    /// If true, new nodes cannot be added.
    /// </summary>
    public bool IsSealed
    {
        get; private set;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="conversationTitle">
    /// The title of the conversation to inject the nodes into.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationInjection(string conversationTitle)
    {
        ConversationTitle = conversationTitle ??
            throw new ArgumentNullException(nameof(conversationTitle));
        Nodes = new(_nodes);
    }

    /// <summary>
    /// Add a node to the list.
    /// </summary>
    /// <param name="node">
    /// The node to add.
    /// </param>
    /// <returns>
    /// The same ConversationInjection object for chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the object is sealed.
    /// </exception>
    public ConversationInjection AddNode(ConversationNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        if (IsSealed)
        {
            throw new InvalidOperationException("Cannot add new nodes after the object has " +
                "been sealed.");
        }

        _nodes.Add(node);
        return this;
    }

    /// <summary>
    /// Seal the object. New nodes cannot be added once this is called.
    /// This is automatically called when the injection is registered.
    /// </summary>
    public void Seal()
    {
        IsSealed = true;
    }

    /// <summary>
    /// Register this <c cref="ConversationInjection">ConversationInjection</c> in the
    /// <c cref="ConversationRegistry">ConversationRegistry</c>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if registration is closed.
    /// </exception>
    /// <seealso cref="ConversationRegistry.RegisterInjection"/>
    public void Register()
    {
        ConversationRegistry.RegisterInjection(this);
    }
}
