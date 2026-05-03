using Il2CppPixelCrushers.DialogueSystem;
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
        get; init;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="name">
    /// The name of the node.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNodeModdedNameSelector(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public override DialogueEntry Resolve(
        DialogueConversation conversation,
        IReadOnlyCollection<InjectedConversationNode> nodes)
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
