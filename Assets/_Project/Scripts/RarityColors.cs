using UnityEngine;

public static class RarityColors
{
    public static Color Of(Rarity r) => r switch
    {
        Rarity.Common    => new Color(0.85f, 0.85f, 0.85f),
        Rarity.Rare      => new Color(0.30f, 0.55f, 1f),
        Rarity.Epic      => new Color(0.65f, 0.35f, 1f),
        Rarity.Legendary => new Color(1f, 0.80f, 0.20f),
        _                => Color.white
    };
}