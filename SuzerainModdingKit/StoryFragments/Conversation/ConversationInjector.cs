using System.Globalization;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

internal static class ConversationInjector
{
    private static readonly List<string> _conversationsInjected = [];

    private static void CreateNodeLinks(InjectedNode node, IReadOnlyList<InjectedNode> nodes)
    {
        List<DialogueEntry> hookedEntries = [];
        for (int i = 0; i < node.Node.HookSelectors.Count; i++)
        {
            ConversationNodeSelector selector = node.Node.HookSelectors[i];
            DialogueEntry entry = selector.Resolve(node.Conversation, nodes);
            if (entry == null)
            {
                Melon<Core>.Logger.Warning("Failed to resolve hook " +
                    $"{i.ToString(CultureInfo.InvariantCulture)} for '{node.Node.Name}'.");
                continue;
            }
            hookedEntries.Add(entry);
        }

        List<int> nextEntryIDs = [];
        for (int i = 0; i < node.Node.NextNodeSelectors.Count; i++)
        {
            ConversationNodeSelector selector = node.Node.NextNodeSelectors[i];
            DialogueEntry entry = selector.Resolve(node.Conversation, nodes);
            if (entry == null)
            {
                Melon<Core>.Logger.Warning("Failed to resolve next node " +
                    $"{i.ToString(CultureInfo.InvariantCulture)} for '{node.Node.Name}'.");
                continue;
            }
            nextEntryIDs.Add(entry.id);
        }

        foreach (int nextID in nextEntryIDs)
        {
            Func<Link, bool> exists = (link) => link.destinationDialogueID == nextID;
            bool linkExists = node.Entry.outgoingLinks.Exists(exists);
            if (linkExists)
            {
                Melon<Core>.Logger.Warning("Found duplicate outgoing links " +
                    $"from node '{node.Node.Name}'.");
                continue;
            }

            Link nextLink = new(
                node.Conversation.id, node.Entry.id,
                node.Conversation.id, nextID);
            node.Entry.outgoingLinks.Add(nextLink);
        }

        foreach (DialogueEntry parent in hookedEntries)
        {
            Func<Link, bool> exists = (link) => link.destinationDialogueID == node.Entry.id;
            bool linkExists = parent.outgoingLinks.Exists(exists);
            if (linkExists)
            {
                Melon<Core>.Logger.Warning("Found duplicate incoming links " +
                    $"to node '{node.Node.Name}'.");
                continue;
            }

            Link parentToNewEntryLink = new(
                node.Conversation.id, parent.id,
                node.Conversation.id, node.Entry.id)
            {
                // Set priority to high to ensure that injected nodes take priority over vanilla nodes.
                // For dialogue lines, Dialogue System only chooses the first valid link with the
                // highest priority, so this ensures that injected dialogue lines will be chosen over
                // vanilla ones. For choice nodes, all links will be shown, so this doesn't matter.
                priority = ConditionPriority.High,
            };
            parent.outgoingLinks.Add(parentToNewEntryLink);
        }
    }

    private static void LinkInjectedNodes(List<InjectedNode> nodes)
    {
        foreach (InjectedNode node in nodes)
        {
            CreateNodeLinks(node, nodes);
        }
    }

    private static InjectedNode InjectNode(ConversationNode node, DialogueConversation conversation)
    {
        int? speakerID = node.SpeakerSelector?.Resolve();
        if (speakerID == null && !node.IsChoice)
        {
            Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                "Speaker character could not be resolved.");
            return null;
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

        conversation.dialogueEntries.Add(newEntry);

        return new InjectedNode(node, newEntry, conversation);
    }

    public static void LoadInjections(DialogueConversation conversation)
    {
        if (_conversationsInjected.Contains(conversation.Title, StringComparer.Ordinal))
        {
            return;
        }
        _conversationsInjected.Add(conversation.Title);

        List<InjectedNode> injectedNodes = [];
        foreach (ConversationInjection injection in ConversationRegistry.Injections)
        {
            if (!injection.ConversationTitle.Equals(conversation.Title, StringComparison.Ordinal))
            {
                continue;
            }

            foreach (ConversationNode node in injection.Nodes)
            {
                InjectedNode injected = InjectNode(node, conversation);
                if (injected != null)
                {
                    injectedNodes.Add(injected);
                }
            }
        }

        LinkInjectedNodes(injectedNodes);
    }
}
