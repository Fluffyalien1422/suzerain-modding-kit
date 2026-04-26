# Suzerain Implementation Info

A non-exhaustive list of important info about Suzerain's implementation.

This is intended for Suzerain Modding Kit contributors. If you are not a contributor, you don't need to read this.

## Documentation Links

Suzerain is built using [Unity](https://docs.unity.com/en-us), [Dialogue System for Unity](https://www.pixelcrushers.com/dialogue_system/manual2x/html/), and [articy:draft](https://www.articy.com/help/adx/). Looking at the documentation for those tools will be helpful when developing Suzerain Modding Kit.

## Lua and articy:expresso

You might notice that are two types of syntax used in Suzerain's inline scripts.

- (Examples from Suzerain v3.1.0.1.153)
- `userScript` from node 105 in conversation `Turn01_EnT_EconomicOverview`: `Variable["GameCondition.Turn01_EnT_EconomicOverview"] = true;`
- `StoryFragmentProperties.OnStoryFragmentEndInstruction` from conversation `Turn01_EnT_EconomicOverview`: `BaseGame.Panel_EconomicStability = true`

The first script is Lua, used by Dialogue System. The second is [articy:expresso](https://www.articy.com/help/adx/Scripting_in_articy.html), used by articy:draft.

The most important difference is that articy:expresso does not recognize custom variables, while Dialogue System's Lua does. Wherever articy:expresso is used, we will have to find a workaround to edit custom variables. For example, we use the `OnBillSigned` and `OnBillVetoed` C# events to modify variables after a bill is signed/vetoed instead of `SignVariables` and `VetoVariables` because those properties are articy:expresso scripts.

