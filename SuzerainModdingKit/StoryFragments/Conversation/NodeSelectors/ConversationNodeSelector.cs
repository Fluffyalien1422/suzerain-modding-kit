using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

/// <summary>
/// An abstract class that selects a node in a conversation.
/// </summary>
public abstract class ConversationNodeSelector
{
    /// <summary>
    /// Select the conversation node.
    /// </summary>
    /// <param name="conversation">
    /// The conversation to search in.
    /// </param>
    /// <param name="nodes">
    /// A read-only list of all injected nodes.
    /// </param>
    /// <returns>
    /// The dialogue entry of the resolved node or null if it could not be found.
    /// </returns>
    public abstract DialogueEntry Resolve(
        DialogueConversation conversation,
        IReadOnlyCollection<InjectedConversationNode> nodes);
}
