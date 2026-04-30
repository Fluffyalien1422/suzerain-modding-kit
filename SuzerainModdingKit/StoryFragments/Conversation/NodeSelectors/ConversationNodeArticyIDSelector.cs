using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

public class ConversationNodeArticyIDSelector : ConversationNodeSelector
{
    public string ArticyID
    {
        get; init;
    }

    public ConversationNodeArticyIDSelector(string articyID)
    {
        ArticyID = articyID ?? throw new ArgumentNullException(nameof(articyID));
    }

    public override DialogueEntry Resolve(
        DialogueConversation conversation,
        IReadOnlyList<InjectedNode> injectedNodes)
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
