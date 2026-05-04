using System.Globalization;
using System.Text;

namespace SuzerainModdingKit.Utils;

public class ConversationNodeSequenceBuilder
{
    private readonly StringBuilder _sequence = new();

    public static bool IsValidInput(string input)
    {
        return !string.IsNullOrEmpty(input) &&
            !input.Contains('(', StringComparison.InvariantCulture) &&
            !input.Contains(')', StringComparison.InvariantCulture) &&
            !input.Contains(';', StringComparison.InvariantCulture);
    }

    public ConversationNodeSequenceBuilder AddConversant(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"AddConversant({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder RemoveConversant(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"RemoveConversant({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder PlaySceneMusic(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"PlaySceneMusic({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder SetDiscordRichPresence(string description)
    {
        if (IsValidInput(description))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"SetRichPresenceData({description});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder Continue()
    {
        _sequence.Append("Continue();");
        return this;
    }

    public ConversationNodeSequenceBuilder WaitForContinue()
    {
        _sequence.Append("WaitForMessage(Continue);");
        return this;
    }

    public override string ToString()
    {
        return _sequence.ToString();
    }
}
