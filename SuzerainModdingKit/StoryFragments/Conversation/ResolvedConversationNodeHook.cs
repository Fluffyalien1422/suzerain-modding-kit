using Il2CppPixelCrushers.DialogueSystem;

namespace SuzerainModdingKit.StoryFragments.Conversation;

internal sealed class ResolvedConversationNodeHook
{
    public ConversationNodeHook Hook
    {
        get;
    }
    public InjectedConversationNode Node
    {
        get;
    }
    public DialogueEntry ResolvedParent
    {
        get;
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
