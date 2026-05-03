using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

// This class is public because it is used by the public class ConversationNodeSelector.
/// <summary>
/// Information about a <c cref="ConversationNode">ConversationNode</c> that has been injected.
/// </summary>
public class InjectedConversationNode
{
    /// <summary>
    /// The <c cref="ConversationNode">ConversationNode</c>.
    /// </summary>
    public ConversationNode Node
    {
        get;
    }
    /// <summary>
    /// The entry in the conversation.
    /// </summary>
    public DialogueEntry Entry
    {
        get;
    }
    /// <summary>
    /// The conversation that the node is injected in.
    /// </summary>
    public DialogueConversation Conversation
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="node">
    /// The <c cref="ConversationNode">ConversationNode</c>.
    /// </param>
    /// <param name="entry">
    /// The entry in the conversation.
    /// </param>
    /// <param name="conversation">
    /// The conversation that the node is injected in.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public InjectedConversationNode(
        ConversationNode node,
        DialogueEntry entry,
        DialogueConversation conversation)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        Conversation = conversation ?? throw new ArgumentNullException(nameof(conversation));
    }
}
