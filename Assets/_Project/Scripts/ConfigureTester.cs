using UnityEngine;

public class ConfigureTester : MonoBehaviour
{
    public FableDefinition fableToApply;   // drag a Fable asset here
    public int level = 1;
    public int stars = 1;

    void Start()
    {
        var unit = GetComponent<Unit>();
        unit.Configure(fableToApply, level, stars);
        Debug.Log($"Configured {unit.name}: HP {unit.maxHealth}, DMG {unit.attackDamage}");
    }
}