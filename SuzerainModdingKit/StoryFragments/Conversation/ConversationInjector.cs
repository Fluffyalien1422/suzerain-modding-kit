using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

internal static class ConversationInjector
{
    private static readonly List<string> _conversationsInjected = [];

    private static void InjectNode(ConversationNode node, DialogueConversation conversation)
    {
        int? parentIDNullable = node.ParentNodeSelector.Resolve(conversation);
        if (parentIDNullable == null)
        {
            Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                "Parent node could not be resolved.");
            return;
        }
        int parentID = (int)parentIDNullable;

        int? nextIDNullable = node.NextNodeSelector.Resolve(conversation);
        if (nextIDNullable == null)
        {
            Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                "Next node could not be resolved.");
            return;
        }
        int nextID = (int)nextIDNullable;

        int? speakerID = node.SpeakerSelector?.Resolve();
        if (speakerID == null && !node.IsChoice)
        {
            Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                "Speaker character could not be resolved.");
            return;
        }

        DialogueEntry parent = conversation.GetDialogueEntry(parentID);
        if (parent == null)
        {
            Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                $"Parent node could not be found in conversation '{conversation.Title}'.");
            return;
        }

        Template template = Template.FromDefault();
        int newID = template.GetNextDialogueEntryID(conversation);

        DialogueEntry newEntry = template.CreateDialogueEntry(newID, conversation.id, node.Name);
        string articyID = ArticyIDGenerator.GenerateArticyID(node.Name);
        newEntry.SetTextField("Articy Id", articyID);
        newEntry.currentLocalizedDialogueText = node.Text;

        // Actor = The person speaking the line.
        // Conversant = The person listening to the line.
        if (node.IsChoice)
        {
            // conversation.ActorID is the player.
            newEntry.ActorID = conversation.ActorID;
            // ConversantID doesn't matter for choices, so just inherit from the conversation.
            newEntry.ConversantID = conversation.ConversantID;
        }
        else
        {
            newEntry.ActorID = speakerID ?? 0;
            // The conversant should be the player.
            newEntry.ConversantID = conversation.ActorID;
        }

        Link nextLink = new(
            conversation.id, newID,
            conversation.id, nextID);
        newEntry.outgoingLinks.Add(nextLink);

        conversation.dialogueEntries.Add(newEntry);

        Link parentToNewEntryLink = new(
            conversation.id, parentID,
            conversation.id, newID)
        {
            // Set priority to high to ensure that injected nodes take priority over vanilla nodes.
            // For dialogue lines, Dialogue System only chooses the first valid link with the
            // highest priority, so this ensures that injected dialogue lines will be chosen over
            // vanilla ones. For choice nodes, all links will be shown, so this doesn't matter.
            priority = ConditionPriority.High,
        };
        parent.outgoingLinks.Add(parentToNewEntryLink);
    }

    private static void LoadInjection(
        ConversationInjection injection,
        DialogueConversation conversation)
    {
        foreach (ConversationNode node in injection.Nodes)
        {
            InjectNode(node, conversation);
        }
    }

    public static void LoadInjections(DialogueConversation conversation)
    {
        if (_conversationsInjected.Contains(conversation.Title, StringComparer.Ordinal))
        {
            return;
        }
        _conversationsInjected.Add(conversation.Title);

        foreach (ConversationInjection injection in ConversationRegistry.Injections)
        {
            if (injection.ConversationTitle.Equals(conversation.Title, StringComparison.Ordinal))
            {
                LoadInjection(injection, conversation);
            }
        }
    }
}
