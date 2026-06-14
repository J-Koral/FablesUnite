// The counter ring: Water -> Fire -> Grass -> Ground -> Electric -> Water.
// A "static" class means we never put it on a GameObject; we just call its method.

public static class ElementChart
{
    // Tuning knobs (GDD: ~+50% strong / -25% weak).
    public const float Strong  = 1.5f;
    public const float Weak    = 0.75f;
    public const float Neutral = 1f;

    // Multiplier applied to the attacker's damage based on the defender's element.
    public static float GetMultiplier(Element attacker, Element defender)
    {
        if (Beats(attacker, defender)) return Strong;
        if (Beats(defender, attacker)) return Weak;
        return Neutral;
    }

    private static bool Beats(Element a, Element b)
    {
        switch (a)
        {
            case Element.Water:    return b == Element.Fire;
            case Element.Fire:     return b == Element.Grass;
            case Element.Grass:    return b == Element.Ground;
            case Element.Ground:   return b == Element.Electric;
            case Element.Electric: return b == Element.Water;
        }
        return false;
    }
}
