using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Identity")]
    public Team team = Team.Defender;
    public Element element = Element.Water;

    [Header("Combat Stats")]
    [Tooltip("Total hit points.")]
    public float maxHealth = 100f;
    [Tooltip("Damage per hit, before the element multiplier.")]
    public float attackDamage = 20f;
    [Tooltip("Attacks per second.")]
    public float attacksPerSecond = 1f;
    [Tooltip("How close an enemy must be (along the lane) to attack it.")]
    public float attackRange = 1.5f;

    [Header("Testing only")]
    [Tooltip("If set, this unit auto-joins that lane on start. Used for units you drop into the scene by hand. Spawned units leave this empty.")]
    [SerializeField] private Lane startingLane;

     [Header("Data (set at spawn)")]
    public FableDefinition definition;

    // Call this right after spawning to stamp stats from data.
    public void Configure(FableDefinition def, int level, int stars)
    {
        definition = def;
        team = def.team;
        element = def.element;

        float mult = (1f + def.perLevelGain * (level - 1)) * (1f + def.perStarGain * (stars - 1));
        maxHealth = def.baseHealth * mult;
        attackDamage = def.baseDamage * mult;
        attacksPerSecond = def.attacksPerSecond;
        attackRange = def.attackRange;

        // re-apply health since Awake already ran on a spawned object
        SendMessage("ResetHealth", SendMessageOptions.DontRequireReceiver);
    }

   // Add a tiny helper so health refreshes after configuring:
   private void ResetHealth() { currentHealth = maxHealth; }


    public Lane Lane { get; private set; }
    public bool IsAlive => currentHealth > 0f;

    private float currentHealth;
    private float cooldownRemaining;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Only fires for hand-placed test units; spawned units already have a lane.
        if (Lane == null && startingLane != null) AssignLane(startingLane);
    }

    public void AssignLane(Lane lane)
    {
        Lane = lane;
        if (Lane != null) Lane.Register(this);
    }

    private void Update()
    {
        if (!IsAlive || Lane == null) return;

        cooldownRemaining -= Time.deltaTime;

        Unit target = Lane.GetNearestEnemy(this);
        if (target == null) return;

        float distance = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (distance <= attackRange && cooldownRemaining <= 0f)
        {
            Strike(target);
            cooldownRemaining = 1f / Mathf.Max(0.01f, attacksPerSecond);
        }
    }

    private void Strike(Unit target)
    {
        float multiplier = ElementChart.GetMultiplier(element, target.element);
        target.TakeDamage(attackDamage * multiplier);
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        currentHealth -= amount;
        if (currentHealth <= 0f) Die();
    }

    private void Die()
    {
        if (Lane != null) Lane.Unregister(this);
        Destroy(gameObject);
    }
}
