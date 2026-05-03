using Il2CppPixelCrushers.DialogueSystem;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// Hook configuration for a <c cref="ConversationNode">ConversationNode</c>.
/// </summary>
public class ConversationNodeHook
{
    /// <summary>
    /// Methods for hooking a <c cref="ConversationNode">ConversationNode</c> to a parent node.
    /// </summary>
    public enum HookMode
    {
        /// <summary>
        /// Split: Break the chain immediately after the target and insert this node in-between.
        /// </summary>
        Split,
        /// <summary>
        /// ConditionGated: Choose the first (sorted by priority) outgoing link with a successful
        /// condition. For choices, all with successful conditions will show.
        /// </summary>
        ConditionGated,
        /// <summary>
        /// Override: Delete all other outgoing links and add this node.
        /// </summary>
        Override,
    }
    /// <summary>
    /// Priorities for hooking a <c cref="ConversationNode">ConversationNode</c> to a parent node.
    /// </summary>
    public enum HookPriority
    {
        /// <summary>
        /// The lowest priority.
        /// </summary>
        Low,
        /// <summary>
        /// The default priority.
        /// </summary>
        Normal,
        /// <summary>
        /// The highest priority.
        /// </summary>
        High,
    }

    /// <summary>
    /// The selector to select the node to hook to.
    /// </summary>
    public ConversationNodeSelector Selector
    {
        get;
    }
    /// <summary>
    /// The mode to use.
    /// </summary>
    public HookMode Mode
    {
        get;
    }
    /// <summary>
    /// The priority of this hook.
    /// </summary>
    public HookPriority Priority
    {
        get;
    }

    internal ConditionPriority ConditionPriority => HookPriorityToConditionPriority(Priority);

    /// <summary>
    /// Create a new instance of this class.
    /// </summary>
    /// <param name="selector">
    /// The selector to select the node to hook to.
    /// </param>
    /// <param name="mode">
    /// Optional: The mode to use. Default: Split.
    /// </param>
    /// <param name="priority">
    /// Optional: The priority of this hook. Default: Normal.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNodeHook(
        ConversationNodeSelector selector,
        HookMode mode = HookMode.Split,
        HookPriority priority = HookPriority.Normal)
    {
        Selector = selector ?? throw new ArgumentNullException(nameof(selector));
        Mode = mode;
        Priority = priority;
    }

    internal static ConditionPriority HookPriorityToConditionPriority(HookPriority priority)
    {
        return priority switch
        {
            HookPriority.Low => ConditionPriority.Low,
            HookPriority.Normal => ConditionPriority.Normal,
            HookPriority.High => ConditionPriority.High,
            _ => ConditionPriority.Normal,
        };
    }
}
