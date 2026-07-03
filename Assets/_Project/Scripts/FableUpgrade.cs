using UnityEngine;

public static class FableUpgrade
{
    public const int XpPerLevel = 100;
    public const int ShardsPerStar = 50;

    public static bool TryLevelUp(OwnedFable o)
    {
        if (o.xp < XpPerLevel) return false;
        o.xp -= XpPerLevel;
        o.level++;
        return true;
    }

    public static bool TryStarUp(OwnedFable o, FableDefinition def)
    {
        if (o.stars >= def.maxStars) return false;
        if (o.shards < ShardsPerStar) return false;
        o.shards -= ShardsPerStar;
        o.stars++;
        return true;
    }

    // At max stars, evolve into the linked definition (resets to lvl 1, 1 star, new identity).
    public static bool TryEvolve(OwnedFable o, FableDatabase db)
    {
        var def = db.Get(o.definitionId);
        if (def == null || def.evolvesInto == null) return false;
        if (o.stars < def.maxStars) return false;
        o.definitionId = def.evolvesInto.name;
        o.level = 1; o.stars = 1;
        return true;
    }
}

