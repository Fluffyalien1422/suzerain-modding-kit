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

    //TODO: what if a node has a circular reference?
    //TODO: what if there is a stack overflow?
    private static List<DialogueEntry> FindFinalNodes(DialogueEntry originEntry)
    {
        DialogueDatabase db = DialogueManager.MasterDatabase;

        Stack<DialogueEntry> stack = new();
        List<DialogueEntry> finalNodes = [];
        stack.Push(originEntry);

        while (stack.Count > 0)
        {
            DialogueEntry entry = stack.Pop();
            if (entry.outgoingLinks.Count == 0)
            {
                finalNodes.Add(entry);
                continue;
            }
            foreach (Link link in entry.outgoingLinks)
            {
                DialogueConversation nextConversation =
                    db.GetConversation(link.destinationConversationID);
                if (nextConversation == null)
                {
                    continue;
                }
                DialogueEntry nextEntry =
                    nextConversation.GetDialogueEntry(link.destinationDialogueID);
                stack.Push(nextEntry);
            }
        }

        return finalNodes;
    }

    private static void HookNode(ResolvedConversationNodeHook resolvedHook)
    {
        ConversationNodeHook hook = resolvedHook.Hook;
        InjectedConversationNode node = resolvedHook.Node;
        DialogueEntry parent = resolvedHook.ResolvedParent;

        Link link = new(
            node.Conversation.id, parent.id,
            node.Conversation.id, node.Entry.id)
        {
            priority = hook.ConditionPriority,
        };

        // ConditionGated: Choose the first (sorted by priority) outgoing link with a successful
        // condition. For choices, all with successful conditions will show.
        // This is the default behavior of Dialogue System, so just add it to the
        // outgoingLinks and let Dialogue System handle it.
        if (hook.Mode == ConversationNodeHook.HookMode.ConditionGated)
        {
            parent.outgoingLinks.Add(link);
            return;
        }

        // Override: Delete all other outgoing links and add this one.
        if (hook.Mode == ConversationNodeHook.HookMode.Override)
        {
            parent.outgoingLinks.Clear();
            parent.outgoingLinks.Add(link);
            return;
        }

        // Split: Break the chain at this point and insert this node in-between.

        // Copy the parent's outgoing links to each of the final nodes in this chain.
        List<DialogueEntry> finalNodes = FindFinalNodes(node.Entry);
        foreach (DialogueEntry finalEntry in finalNodes)
        {
            foreach (Link parentLink in parent.outgoingLinks)
            {
                finalEntry.outgoingLinks.Add(parentLink);
            }
        }

        // Clear the parent's links and add this one, which should eventually link back
        // to the parent's original links.
        parent.outgoingLinks.Clear();
        parent.outgoingLinks.Add(link);
    }

    private static void CreateNodeOutgoingLinks(InjectedConversationNode node, IReadOnlyList<InjectedConversationNode> nodes)
    {
        for (int i = 0; i < node.Node.NextNodes.Count; i++)
        {
            ConversationNodeSelector selector = node.Node.NextNodes[i];
            DialogueEntry entry = selector.Resolve(node.Conversation, nodes);
            if (entry == null)
            {
                Melon<Core>.Logger.Warning("Failed to resolve next node " +
                    $"{i.ToString(CultureInfo.InvariantCulture)} for conversation node " +
                    $"'{node.Node.Name}'.");
                continue;
            }

            Func<Link, bool> exists = (link) => link.destinationDialogueID == entry.id;
            bool linkExists = node.Entry.outgoingLinks.Exists(exists);
            if (linkExists)
            {
                Melon<Core>.Logger.Warning("Found duplicate outgoing links " +
                    $"from conversation node '{node.Node.Name}'.");
                continue;
            }

            Link nextLink = new(
                node.Conversation.id, node.Entry.id,
                node.Conversation.id, entry.id);
            node.Entry.outgoingLinks.Add(nextLink);
        }
    }

    private static List<ResolvedConversationNodeHook> ResolveHooks(IReadOnlyList<InjectedConversationNode> nodes)
    {
        List<ResolvedConversationNodeHook> resolvedHooks = [];
        foreach (InjectedConversationNode node in nodes)
        {
            List<int> resolvedParentIDs = [];

            for (int i = 0; i < node.Node.Hooks.Count; i++)
            {
                ConversationNodeHook hook = node.Node.Hooks[i];
                ConversationNodeSelector selector = hook.Selector;
                DialogueEntry entry = selector.Resolve(node.Conversation, nodes);
                if (entry == null)
                {
                    Melon<Core>.Logger.Warning("Failed to resolve hook " +
                        $"{i.ToString(CultureInfo.InvariantCulture)} for conversation node " +
                        $"'{node.Node.Name}'.");
                    continue;
                }

                // Check if this node hooks to the same parent multiple times.
                if (resolvedParentIDs.Contains(entry.id))
                {
                    Melon<Core>.Logger.Warning($"Conversation node '{node.Node.Name}' has " +
                        "duplicate hooks.");
                    continue;
                }

                // Check if the parent already has a link to this node.
                Func<Link, bool> exists = (link) => link.destinationDialogueID == node.Entry.id;
                bool linkExists = entry.outgoingLinks.Exists(exists);
                if (linkExists)
                {
                    Melon<Core>.Logger.Warning("Found duplicate incoming links " +
                        $"to conversation node '{node.Node.Name}'.");
                    continue;
                }

                resolvedParentIDs.Add(entry.id);
                ResolvedConversationNodeHook resolved = new(hook, node, entry);
                resolvedHooks.Add(resolved);
            }
        }
        return resolvedHooks;
    }

    private static void LinkInjectedNodes(IReadOnlyList<InjectedConversationNode> nodes)
    {
        // Create the outgoing links first. All the outgoing links have to be created before
        // we can create hooks.
        foreach (InjectedConversationNode node in nodes)
        {
            CreateNodeOutgoingLinks(node, nodes);
        }

        // Resolve and create hooks.
        List<ResolvedConversationNodeHook> resolvedHooks = ResolveHooks(nodes);
        resolvedHooks = [.. resolvedHooks.OrderBy(h => h.Hook.Priority)];
        foreach (ResolvedConversationNodeHook hook in resolvedHooks)
        {
            HookNode(hook);
        }
    }

    private static InjectedConversationNode InjectNode(ConversationNode node, DialogueConversation conversation)
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

        return new InjectedConversationNode(node, newEntry, conversation);
    }

    public static void LoadInjections(DialogueConversation conversation)
    {
        if (_conversationsInjected.Contains(conversation.Title, StringComparer.Ordinal))
        {
            return;
        }
        _conversationsInjected.Add(conversation.Title);

        List<InjectedConversationNode> injectedNodes = [];
        foreach (ConversationInjection injection in ConversationRegistry.Injections)
        {
            if (!injection.ConversationTitle.Equals(conversation.Title, StringComparison.Ordinal))
            {
                continue;
            }

            foreach (ConversationNode node in injection.Nodes)
            {
                InjectedConversationNode injected = InjectNode(node, conversation);
                if (injected != null)
                {
                    injectedNodes.Add(injected);
                }
            }
        }

        LinkInjectedNodes(injectedNodes);
    }
}
