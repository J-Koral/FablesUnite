using UnityEngine;

public class TreeController : MonoBehaviour
{
    public TreeConfig config;

    [Tooltip("Minigame spots hit during a hold are counted here, played after the burst.")]
    public int pendingMinigames = 0;

    // Spend one token and resolve one spot. Returns the chosen spot, or null if broke.
    public TreeSpot Pull()
    {
        var p = GameData.I.player;
        if (p.treeTokens <= 0) return null;
        p.treeTokens--;

        // Pity: if we're at the threshold, force a Fable spot.
        if (p.pityCounter >= config.fablePity)
        {
            var fableSpot = config.spots.Find(s => s.type == TreeRewardType.Fable);
            if (fableSpot != null) { Grant(fableSpot); p.pityCounter = 0; return fableSpot; }
        }

        // Weighted pick-one.
        float roll = Random.value * config.TotalWeight();
        foreach (var s in config.spots)
        {
            roll -= Mathf.Max(0f, s.weight);
            if (roll <= 0f) { Grant(s); return s; }
        }
        return null;
    }

    private void Grant(TreeSpot s)
    {
        var p = GameData.I.player;
        switch (s.type)
        {
            case TreeRewardType.Gold:        p.gold += s.amount; break;
            case TreeRewardType.TreeTokens:  p.treeTokens += s.amount; break;
            case TreeRewardType.RaidCoins:   p.raidCoins += s.amount; break;
            case TreeRewardType.FableShards: AddShards(s.amount); break;
            case TreeRewardType.Xp:          AddXpToAll(s.amount); break;
            case TreeRewardType.Fable:       GrantFable(s.fable); break;
            case TreeRewardType.Minigame:    pendingMinigames++; break;   // queued, not interrupting
        }

        // pity advances on every non-Fable pull, resets on a Fable
        if (s.type == TreeRewardType.Fable) p.pityCounter = 0;
        else p.pityCounter++;

        GameData.I.Save();
    }

    private void GrantFable(FableDefinition def)
    {
        if (def == null) return;
        var p = GameData.I.player;
        var existing = p.roster.Find(o => o.definitionId == def.name);
        if (existing != null) existing.shards += 10;     // duplicate → shards
        else p.roster.Add(new OwnedFable(def.name));     // new Fable!
    }

    private void AddShards(int n)
    {
        if (GameData.I.player.roster.Count == 0) return;
        GameData.I.player.roster[0].shards += n;          // simple: shard your first Fable
    }

    private void AddXpToAll(int n)
    {
        foreach (var o in GameData.I.player.roster) o.xp += n;
    }
}