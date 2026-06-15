using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Mover : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("World units per second toward the back line.")]
    public float speed = 1.5f;
    [Tooltip("Minimum gap kept behind a friendly unit ahead (prevents overlap).")]
    public float spacing = 0.8f;

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (!unit.IsAlive || unit.Lane == null) return;

        // 1. If an enemy is within attack range, stop and let Unit fight.
        Unit enemy = unit.Lane.GetNearestEnemy(unit);
        if (enemy != null)
        {
            float distance = Mathf.Abs(enemy.transform.position.x - transform.position.x);
            if (distance <= unit.attackRange) return;
        }

        // 2. If a friendly unit is right ahead, hold position (keep spacing).
        if (unit.Lane.GetNearestAllyAhead(unit, spacing) != null) return;

        // 3. Otherwise advance toward the back line (negative X).
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
