public static class FableStats
{
    public static float Multiplier(FableDefinition def, int level, int stars)
        => (1f + def.perLevelGain * (level - 1)) * (1f + def.perStarGain * (stars - 1));

    public static float Health(FableDefinition def, int level, int stars)
        => def.baseHealth * Multiplier(def, level, stars);
    public static float Damage(FableDefinition def, int level, int stars)
        => def.baseDamage * Multiplier(def, level, stars);
}
