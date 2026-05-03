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
        get; init;
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
    public ConversationInjection AddNode(ConversationNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        _nodes.Add(node);
        return this;
    }

    /// <summary>
    /// Clones this object.
    /// </summary>
    /// <returns>
    /// A clone of this object.
    /// </returns>
    public ConversationInjection Clone()
    {
        ConversationInjection cloned = new(ConversationTitle);
        foreach (ConversationNode node in _nodes)
        {
            _ = cloned.AddNode(node);
        }
        return cloned;
    }
}
