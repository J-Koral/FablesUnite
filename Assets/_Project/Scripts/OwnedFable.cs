[System.Serializable]
public class OwnedFable
{
    public string definitionId;   // matches FableDefinition.name, so it survives saving
    public int level = 1;
    public int stars = 1;
    public int xp = 0;            // toward next level
    public int shards = 0;        // duplicates, toward next star

    public OwnedFable() { }
    public OwnedFable(string id) { definitionId = id; }
}
