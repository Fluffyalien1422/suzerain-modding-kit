using Il2CppPixelCrushers.DialogueSystem;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

/// <summary>
/// Selects a conversation node by its Articy ID.
/// </summary>
public class ConversationNodeArticyIDSelector : ConversationNodeSelector
{
    /// <summary>
    /// The Articy ID of the node.
    /// </summary>
    public string ArticyID
    {
        get;
    }
    /// <summary>
    /// Optional: The name of the conversation to search for the node in. If null, searches
    /// the current conversation.
    /// </summary>
    public string ConversationName
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="articyID">
    /// The Articy ID of the node.
    /// </param>
    /// <param name="conversationName">
    /// Optional: The name of the conversation to search for the node in. If null, searches
    /// the current conversation.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNodeArticyIDSelector(string articyID, string conversationName = null)
    {
        ArticyID = articyID ?? throw new ArgumentNullException(nameof(articyID));
        ConversationName = conversationName;
    }

    public override DialogueEntry Resolve(
        DialogueConversation currentConversation,
        IReadOnlyCollection<InjectedConversationNode> nodes)
    {
        ArgumentNullException.ThrowIfNull(currentConversation);
        ArgumentNullException.ThrowIfNull(nodes);

        DialogueConversation conversation =
            DialogueUtils.GetConversation(ConversationName) ?? currentConversation;

        foreach (DialogueEntry entry in conversation.dialogueEntries)
        {
            string entryArticyID = Field.LookupValue(entry.fields, "Articy Id");
            if (string.Equals(entryArticyID, ArticyID, StringComparison.Ordinal))
            {
                return entry;
            }
        }

        return null;
    }
}
