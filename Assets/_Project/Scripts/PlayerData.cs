using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int treeTokens = 20;
    public int gold = 0;
    public int raidCoins = 0;
    public int pityCounter = 0;     // pulls since the last Fable
    public int chapterLevel = 1;
    public List<OwnedFable> roster = new List<OwnedFable>();
}