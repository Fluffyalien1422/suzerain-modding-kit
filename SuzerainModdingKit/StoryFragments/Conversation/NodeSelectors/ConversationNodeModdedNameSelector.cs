using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

public class ConversationNodeModdedNameSelector : ConversationNodeSelector
{
    public string Name
    {
        get; init;
    }

    public ConversationNodeModdedNameSelector(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public override DialogueEntry Resolve(
        DialogueConversation conversation,
        IReadOnlyList<InjectedConversationNode> nodes)
    {
        foreach (InjectedConversationNode node in nodes)
        {
            if (node.Node.Name.Equals(Name, StringComparison.Ordinal))
            {
                return node.Entry;
            }
        }
        return null;
    }
}
