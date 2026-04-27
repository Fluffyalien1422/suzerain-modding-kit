using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

public abstract class ConversationNodeSelector
{
    public abstract int? Resolve(DialogueConversation conversation);
}
