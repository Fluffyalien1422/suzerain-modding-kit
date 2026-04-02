using Il2Cpp;

namespace SuzerainModdingKit.StoryFragments;

public class CustomBillData
{
    public string Name
    {
        get; init;
    }
    public string AssignedTokenName
    {
        get; init;
    }
    public string Title
    {
        get; init;
    }
    public string Description
    {
        get; init;
    }
    public string HubTitle
    {
        get; init;
    }
    public string HubDescription
    {
        get; init;
    }

    public CustomBillData(
        string name,
        string assignedTokenName,
        string title,
        string description,
        string hubTitle,
        string hubDescription)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AssignedTokenName = assignedTokenName ?? throw new ArgumentNullException(nameof(assignedTokenName));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        HubTitle = hubTitle ?? throw new ArgumentNullException(nameof(hubTitle));
        HubDescription = hubDescription ?? throw new ArgumentNullException(nameof(hubDescription));
    }

    internal BillData ToSuzerainBillData()
    {
        BillProperties customBillProperties = new()
        {
            Title = Title,
            Description = Description,
            HubTitle = HubTitle,
            HubDescription = HubDescription,
            // These properties don't seem to do anything.
            // Use Events.OnBillSigned/Vetoed instead.
            SignVariables = "",
            VetoVariables = "",
        };
        BillData customBillData = new()
        {
            AppBundleProperties = new AppBundleProperties()
            {
                AppBundle = "AppBundle_Main",
                StoryPacks = InternalUtils.CreateIl2CppList(["StoryPack_Main"]),
            },
            AssignedTokenProperties = new AssignedTokenProperties()
            {
                AssignedToken = AssignedTokenName,
            },
            BillProperties = customBillProperties,
            NameInDatabase = Name,
            Path = "Sordland/Bills",
            StoryFragmentProperties = new StoryFragmentProperties()
            {
                IsDone = false,
                OnStoryFragmentBeginInstruction = "",
                OnStoryFragmentEndInstruction = "",
            },
            TagsProperties = new TagsProperties(),
        };
        return customBillData;
    }
}
