using HarmonyLib;
using Il2Cpp;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

[HarmonyPatch(typeof(ConversationPanel), nameof(ConversationPanel.Setup))]
internal static class ConversationPanel_Setup_Patch
{
    // Use Prefix to modify the conversation before it starts.
    public static void Prefix(ConversationData conversationData)
    {
        string title = conversationData.ConversationProperties.Dialogue;

        DialogueConversation conversation = DialogueUtils.GetConversation(title);
        if (conversation == null)
        {
            return;
        }

        ConversationInjector.PatchConversation(conversation);
    }
}
