using Il2CppPixelCrushers.DialogueSystem;

namespace SuzerainModdingKit.Utils;

public static class DialogueActorFinder
{
    public static int? FindActorIDByArticyID(string articyID)
    {
        DialogueDatabase db = DialogueManager.MasterDatabase;
        foreach (Actor actor in db.actors)
        {
            string actorArticyID = actor.LookupValue("Articy ID");
            if (actorArticyID.Equals(articyID, StringComparison.Ordinal))
            {
                return actor.id;
            }
        }
        return null;
    }

    public static int? FindActorIDByName(string name)
    {
        DialogueDatabase db = DialogueManager.MasterDatabase;
        return db.GetActor(name)?.id;
    }
}
