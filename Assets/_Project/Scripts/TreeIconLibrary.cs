using UnityEngine;

public class TreeIconLibrary : MonoBehaviour
{
    public Sprite gold, xp, treeTokens, raidCoins, fableShards, fable, minigame;

    public Sprite Get(TreeRewardType type)
    {
        switch (type)
        {
            case TreeRewardType.Gold:        return gold;
            case TreeRewardType.Xp:          return xp;
            case TreeRewardType.TreeTokens:  return treeTokens;
            case TreeRewardType.RaidCoins:   return raidCoins;
            case TreeRewardType.FableShards: return fableShards;
            case TreeRewardType.Fable:       return fable;
            case TreeRewardType.Minigame:    return minigame;
        }
        return null;
    }
}
