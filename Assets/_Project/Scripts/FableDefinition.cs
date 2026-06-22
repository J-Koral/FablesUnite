using UnityEngine;

[CreateAssetMenu(fileName = "Fable", menuName = "Fables Unite/Fable Definition")]
public class FableDefinition : ScriptableObject
{
    [Header("Identity")]
    public string displayName = "New Fable";
    public Element element = Element.Water;
    public Rarity rarity = Rarity.Common;
    public Team team = Team.Defender;

    [Header("Base stats (level 1, 1 star)")]
    public float baseHealth = 100f;
    public float baseDamage = 20f;
    public float attacksPerSecond = 1f;
    public float attackRange = 1.5f;

    [Header("Progression")]
    public int maxStars = 5;
    [Tooltip("Stat gain per level, e.g. 0.08 = +8% per level.")]
    public float perLevelGain = 0.08f;
    [Tooltip("Stat gain per star, e.g. 0.15 = +15% per star.")]
    public float perStarGain = 0.15f;

    [Header("Evolution (optional)")]
    [Tooltip("At max stars, this Fable can evolve into this one. Leave empty for none.")]
    public FableDefinition evolvesInto;

    [Header("Art")]
    public Sprite art;
    public Color placeholderColor = Color.white;
}
