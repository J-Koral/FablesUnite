using UnityEngine;

public class DevTools : MonoBehaviour
{
    public void GiveTokens()
    {
        GameData.I.player.treeTokens += 100;
        GameData.I.Save();
        Debug.Log("Gave 100 tokens. Now: " + GameData.I.player.treeTokens);
    }

    public void GiveGold()
    {
        GameData.I.player.gold += 1000;
        GameData.I.Save();
    }

    public void GiveShards()
    {
        if (GameData.I.player.roster.Count > 0)
            GameData.I.player.roster[0].shards += 100;
        GameData.I.Save();
    }
}