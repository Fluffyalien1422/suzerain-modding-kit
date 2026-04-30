using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

public class InjectedNode
{
    public ConversationNode Node
    {
        get; init;
    }
    public DialogueEntry Entry
    {
        get; init;
    }
    public DialogueConversation Conversation
    {
        get; init;
    }

    public InjectedNode(
        ConversationNode node,
        DialogueEntry entry,
        DialogueConversation conversation)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        Entry = entry ?? throw new ArgumentNullException(nameof(entry));
        Conversation = conversation ?? throw new ArgumentNullException(nameof(conversation));
    }
}
