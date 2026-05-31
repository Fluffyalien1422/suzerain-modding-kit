using MelonLoader;
using SuzerainModdingKit.Save;
using SuzerainModdingKit.StoryFragments.Conversation;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "0.1.0", "Fluffyalien1422", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]
// (also update version in SmkInfo.cs)

namespace SuzerainModdingKit;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg(
            $"Suzerain Modding Kit version: {SmkInfo.ModVersion}, " +
            $"Suzerain version: {Application.version}, " +
            $"Target Suzerain version: {SmkInfo.TargetSuzerainVersion}.");

        if (!string.Equals(
            Application.version,
            SmkInfo.TargetSuzerainVersion,
            StringComparison.Ordinal))
        {
            LoggerInstance.Warning(
                $"Expected Suzerain version {SmkInfo.TargetSuzerainVersion}, " +
                $"but got {Application.version}. Suzerain Modding Kit may not work properly.");
        }

        LoggerInstance.Msg("Cleaning up mod saves.");
        SaveManager.CleanupOrphanedModSaves();

        //// Target 'Sordland/Turn02/Personal_Funeral', which is Bernard Circas' funeral.
        //new ConversationInjection("Sordland/Turn02/Personal_Funeral")
        //    // Add a node to the injection.
        //    .AddNode(new ConversationNode(
        //        // The unique identifier of this node.
        //        name: "ExampleMod.PetrHello",
        //        // The text of this node.
        //        text: "Hello from Suzerain Modding Kit!",
        //        // Select the character that should speak this line.
        //        speakerSelector: new CharacterNameSelector("Petr Vectern"),
        //        // Which nodes should this node "hook" or attach to?
        //        hooks: [
        //            new ConversationNodeHook(
        //                // '0x0100000400008561' is "Dark clouds were looming over Deyr..."
        //                selector: new ConversationNodeArticyIDSelector("0x0100000400008561"),
        //                mode: ConversationNodeHook.HookMode.Override),
        //        ],
        //        // Which nodes should this node continue on to?
        //        nextNodes: [
        //            new ConversationNodeModdedNameSelector("ExampleMod.PlayerHeyPetr"),
        //            new ConversationNodeModdedNameSelector("ExampleMod.PlayerHelloPetr"),
        //        ]))
        //    .AddNode(new ConversationNode(
        //        name: "ExampleMod.PlayerHeyPetr",
        //        text: "Hey Petr!"
        //        // 'speakerSelector' is omitted here and the next node,
        //        // which means that it will be a choice (the player speaks it).
        //        ))
        //    .AddNode(new ConversationNode(
        //        name: "ExampleMod.PlayerHelloPetr",
        //        text: "Hello Petr.",
        //        nextNodes: [
        //            // Narrator: "Evening descended on the capital as..."
        //            new ConversationNodeArticyIDSelector("0x010000640000BBDB", "Sordland/Turn04/Personal_LucianChessMatch"),
        //        ]))
        //    .Register();

        LoggerInstance.Msg("Pre-initialization complete.");
    }

    public override void OnLateInitializeMelon()
    {
        ConversationRegistry.CloseRegistration();
        LoggerInstance.Msg("Conversation registration closed.");

        LoggerInstance.Msg("Initialized.");
    }

    public override void OnUpdate()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null)
        {
            return;
        }

        if (kb.ctrlKey.isPressed && kb.dKey.wasPressedThisFrame)
        {
            DebugOverlay.SetVisibility(value: !DebugOverlay.IsShowing());
        }
    }

    public override void OnGUI()
    {
        DebugOverlay.Update();
    }
}
