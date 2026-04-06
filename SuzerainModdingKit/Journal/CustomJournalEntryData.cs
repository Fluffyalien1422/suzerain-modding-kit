using Il2Cpp;

namespace SuzerainModdingKit.Journal;

public class CustomJournalEntryData
{
    public string Name
    {
        get; init;
    }
    public string Description
    {
        get; init;
    }
    public int TurnNum
    {
        get; init;
    }

    public CustomJournalEntryData(string name, string description, int turnNum)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        TurnNum = turnNum;
    }

    internal JournalEntryData ToSuzerainJournalEntryData()
    {
        JournalEntryProperties properties = new()
        {
            Description = $"[MOD] {Description}",
            TurnNo = TurnNum,
        };
        JournalEntryData data = new()
        {
            AppBundleProperties = new AppBundleProperties()
            {
                AppBundle = "AppBundle_Main",
                StoryPacks = InternalUtils.CreateIl2CppList(["StoryPack_Main"]),
            },
            JournalEntryProperties = properties,
            NameInDatabase = Name,
            Path = "Sordland/Journal Entries",
            TagsProperties = new TagsProperties(),
        };
        return data;
    }
}
