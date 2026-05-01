using Il2CppPixelCrushers.DialogueSystem;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

namespace SuzerainModdingKit.StoryFragments.Conversation;

public class ConversationNodeHook
{
    public enum HookMode
    {
        Split,
        ConditionGated,
        Override,
    }
    public enum HookPriority
    {
        Low,
        Normal,
        High,
    }

    public ConversationNodeSelector Selector
    {
        get; init;
    }
    public HookMode Mode
    {
        get; init;
    }
    public HookPriority Priority
    {
        get; init;
    }

    internal ConditionPriority ConditionPriority => HookPriorityToConditionPriority(Priority);

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
