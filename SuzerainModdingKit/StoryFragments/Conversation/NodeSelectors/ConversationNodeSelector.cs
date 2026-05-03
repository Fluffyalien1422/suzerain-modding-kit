using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

public abstract class ConversationNodeSelector
{
    public abstract DialogueEntry Resolve(
        DialogueConversation conversation,
        IReadOnlyList<InjectedConversationNode> nodes);
}
