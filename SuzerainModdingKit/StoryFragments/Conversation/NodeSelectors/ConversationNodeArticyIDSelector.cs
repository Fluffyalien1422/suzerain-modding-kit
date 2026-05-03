using Il2CppPixelCrushers.DialogueSystem;
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
        get; init;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="articyID">
    /// The Articy ID of the node.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNodeArticyIDSelector(string articyID)
    {
        ArticyID = articyID ?? throw new ArgumentNullException(nameof(articyID));
    }

    public override DialogueEntry Resolve(
        DialogueConversation conversation,
        IReadOnlyCollection<InjectedConversationNode> nodes)
    {
        foreach (DialogueEntry entry in conversation.dialogueEntries)
        {
            string entryArticyID = Field.LookupValue(entry.fields, "Articy Id");
            if (entryArticyID.Equals(ArticyID, StringComparison.Ordinal))
            {
                return entry;
            }
        }
        return null;
    }
}
