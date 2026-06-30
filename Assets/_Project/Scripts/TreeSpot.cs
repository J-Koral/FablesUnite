using UnityEngine;

public enum TreeRewardType { Gold, TreeTokens, RaidCoins, FableShards, Fable, Xp, Minigame }

[System.Serializable]
public class TreeSpot
{
    public string label = "Reward";
    public TreeRewardType type = TreeRewardType.Gold;
    public Rarity rarity = Rarity.Common;
    public int amount = 10;
    [Tooltip("Used only when type == Fable.")]
    public FableDefinition fable;
    [Tooltip("Relative weight. Higher = more likely. Odds % are computed from these.")]
    public float weight = 10f;
}
