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
    /// <param name="currentConversation">
    /// The current conversation.
    /// </param>
    /// <returns>
    /// The dialogue entry of the resolved node or null if it could not be found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// May throw if required arguments are null.
    /// </exception>
    public abstract DialogueEntry Resolve(DialogueConversation currentConversation);
}
