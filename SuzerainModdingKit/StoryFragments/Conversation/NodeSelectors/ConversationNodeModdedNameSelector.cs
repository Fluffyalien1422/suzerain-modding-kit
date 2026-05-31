using Il2CppPixelCrushers.DialogueSystem;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

/// <summary>
/// Selects a modded conversation node by its name.
/// </summary>
public class ConversationNodeModdedNameSelector : ConversationNodeSelector
{
    /// <summary>
    /// The name of the node.
    /// </summary>
    public string Name
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
    /// <param name="name">
    /// The name of the node.
    /// </param>
    /// <param name="conversationName">
    /// Optional: The name of the conversation to search for the node in. If null, searches
    /// the current conversation.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNodeModdedNameSelector(string name, string conversationName = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ConversationName = conversationName;
    }

    public override DialogueEntry Resolve(DialogueConversation currentConversation)
    {
        ArgumentNullException.ThrowIfNull(currentConversation);

        DialogueConversation conversation =
            DialogueUtils.GetConversation(ConversationName) ?? currentConversation;

        foreach (DialogueEntry entry in conversation.dialogueEntries)
        {
            string entryNodeName = Field.LookupValue(entry.fields, "SuzerainModdingKit.NodeName");
            if (string.Equals(entryNodeName, Name, StringComparison.Ordinal))
            {
                return entry;
            }
        }

        return null;
    }
}
