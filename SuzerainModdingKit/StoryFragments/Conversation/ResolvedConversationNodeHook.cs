using Il2CppPixelCrushers.DialogueSystem;

namespace SuzerainModdingKit.StoryFragments.Conversation;

internal sealed class ResolvedConversationNodeHook
{
    public ConversationNodeHook Hook
    {
        get; init;
    }
    public InjectedConversationNode Node
    {
        get; init;
    }
    public DialogueEntry ResolvedParent
    {
        get; init;
    }

    public ResolvedConversationNodeHook(
        ConversationNodeHook hook,
        InjectedConversationNode node,
        DialogueEntry resolvedParent)
    {
        Hook = hook ?? throw new ArgumentNullException(nameof(hook));
        Node = node ?? throw new ArgumentNullException(nameof(node));
        ResolvedParent = resolvedParent ?? throw new ArgumentNullException(nameof(resolvedParent));
    }
}
